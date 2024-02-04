using BpCop.DataProviders.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace BpCop.DataProviders
{
    public static class DatabaseProvider
    {
        private static XDocument BuildXDocument<T>(IEnumerable<T> data) => new(new XElement("root", data.Select(s => new XElement("param", s))));

        public static string DecryptConnectionString(string connectionString, string certThumbprint, string certStoreName, string certStoreLocation)
        {
            if (!Enum.TryParse(certStoreName, true, out StoreName storeName))
            {
                throw new InvalidEnumArgumentException("Certificate store name invalid");
            }
            if (!Enum.TryParse(certStoreLocation, true, out StoreLocation storeLocation))
            {
                throw new InvalidEnumArgumentException("Certificate store location invalid");
            }
            using var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var cert = store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false).OfType<X509Certificate2>().SingleOrDefault() ?? throw new InvalidEnumArgumentException($"Certificate with thumbprint '{certThumbprint}' not found");
            using RSA rsa = cert.GetRSAPrivateKey() ?? throw new InvalidEnumArgumentException("Certificate has no private key");
            var connectionStringDecrypted = Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(connectionString), RSAEncryptionPadding.OaepSHA1));
            store.Close();

            return connectionStringDecrypted;
        }

        public static string BuildConnectionString(string dataSource, string initalCatalog, string userId, string password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initalCatalog
            };

            if (string.IsNullOrEmpty(userId))
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = userId;
                builder.Password = password;
            }

            return builder.ConnectionString;
        }

        public static IEnumerable<AssetInformation> GetAssetsInformation(string connectionString)
        {
            var groupRelations = new Dictionary<Guid, Tuple<string, SqlGuid>>();
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var groupcommand = new SqlCommand("SELECT BPAGroup.id, BPAGroup.name, BPAGroupGroup.groupid AS ParentId " +
                "FROM BPAGroup LEFT OUTER JOIN BPAGroupGroup ON BPAGroup.id = BPAGroupGroup.memberid", connection);
            var reader = groupcommand.ExecuteReader();
            while (reader.Read())
            {
                groupRelations.Add(reader.GetGuid(0), Tuple.Create(reader.GetString(1), reader.GetSqlGuid(2)));
            }
            reader.Close();

            IEnumerable<string> buildPath(SqlGuid g)
            {
                while (!g.IsNull)
                {
                    var group = groupRelations[g.Value];
                    yield return group.Item1;
                    g = group.Item2;
                }
            }

            using var assetcommand = new SqlCommand("SELECT BPAProcess.processid, BPAProcess.ProcessType, BPAProcess.name, BPAGroupProcess.groupid " +
                "FROM BPAProcess INNER JOIN " +
                "BPAGroupProcess ON BPAProcess.processid = BPAGroupProcess.processid", connection);

            reader = assetcommand.ExecuteReader();
            while (reader.Read())
            {
                yield return new AssetInformation(
                    ProcessType: reader.GetString(1),
                    ProcessId: reader.GetGuid(0),
                    Path: $"{string.Join("/", buildPath(reader.GetSqlGuid(3)).Reverse())}/{reader.GetString(2)}"
                );
            }
            reader.Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "TA: 05.01.2024 - variable data passed by xml parameter, not by dynamic sql")]
        public static IEnumerable<ProcessInformation> GetProcesses(string connectionString, string likeFilter, IEnumerable<string> names, IEnumerable<Guid> ids)
        {
            var commandText = new StringBuilder("SELECT BPAProcess.processid, BPAProcess.ProcessType, BPAProcess.name, BPAProcess.AttributeID, BPAProcess.processxml FROM BPAProcess");
            SqlParameter? parameter = null;
            if (!string.IsNullOrEmpty(likeFilter))
            {
                commandText.Append(" WHERE BPAProcess.name LIKE @likeFilter");
                parameter = new SqlParameter("@likeFilter", likeFilter)
                {
                    SqlDbType = System.Data.SqlDbType.Text
                };
            }
            else
            {
                if (names is not null && names.Any())
                {
                    commandText.Append(" WHERE BPAProcess.name IN (select t.n.value('text()[1]','nvarchar(max)') from @xml.nodes('/root/param') as t(n))");
                    parameter = new SqlParameter("@xml", BuildXDocument(names).CreateReader())
                    {
                        SqlDbType = System.Data.SqlDbType.Xml
                    };
                }
                else
                {
                    if (ids is not null && ids.Any())
                    {
                        commandText.Append(" WHERE BPAProcess.processid IN (select t.n.value('text()[1]','nvarchar(max)') from @xml.nodes('/root/param') as t(n))");
                        parameter = new SqlParameter("@xml", BuildXDocument(ids).CreateReader())
                        {
                            SqlDbType = System.Data.SqlDbType.Xml
                        };
                    }
                }
            }

            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var command = new SqlCommand(commandText.ToString(), connection);
            if (parameter is not null)
            {
                command.Parameters.Add(parameter);
            }
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new ProcessInformation(
                    ProcessId: reader.GetGuid(0),
                    ProcessType: reader.GetString(1),
                    Name: reader.GetString(2),
                    IsPublished: reader.GetInt32(3) == 2 && reader.GetString(1) == "P",
                    ProcessXml: reader.GetString(4)
                );
            }
            reader.Close();
        }
    }
}
