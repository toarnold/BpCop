using BpCop.Console.Common.Output;
using BpCop.Console.Common.Parameters;
using BpCop.DataProviders;
using BpCop.Rules;
using BpCop.Rules.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BpCop.Console.Common
{
    public class Functions(TextWriter twout, TextWriter twerror)
    {
        internal sealed record AssetInformation(string ProcessXml, Guid ProcessId, bool IsPublished);

        private static Engine BuildEngine(MinimalOptions options)
        {
            return new Engine(options.RuleFolders
                    .Select(p => Path.IsPathRooted(p) && !Path.GetPathRoot(p)!.Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) ? p : Path.Combine(Assembly.GetExecutingAssembly().Location, "..", p))
                    .Where(Directory.Exists));
        }

        private static string GetCustomParameter(MinimalOptions options) => !string.IsNullOrEmpty(options.NamedCustomParameter)
                ? ConfigurationManager.AppSettings[options.NamedCustomParameter] ?? options.CustomParameter
                : options.CustomParameter;

        private bool CheckAssets(IEnumerable<AssetInformation> assets, BaseOptions baseOptions)
        {
            using var engine = BuildEngine(baseOptions);
            var findings = new List<FindingInformation>();
            var justifications = new List<JustificationInformation>();
            bool processed = false;

            // Check rule existance
            foreach (var ruleName in baseOptions.Whitelist
                                    .Concat(baseOptions.Blacklist)
                                    .Distinct()
                                    .Where(w => !engine.HasRule(w)))
            {
                twerror.WriteLine($"Rule '{ruleName}' does not exist - ignoring ...");
            }

            foreach (var asset in assets)
            {
                processed = true;
                try
                {

                    var result = engine.CheckRules(asset.ProcessXml, asset.ProcessId, asset.IsPublished, baseOptions.Whitelist, baseOptions.Blacklist, GetCustomParameter(baseOptions));

                    findings.AddRange(baseOptions.JustificationHandling == Justification.Ignore ? result.Findings : result.Findings.Where(w => string.IsNullOrEmpty(w.JustificationLevel)));
                    justifications.AddRange(result.Justifications);
                }
                catch (Exception e)
                {
                    twerror.WriteLine($"Error analysing '{asset.ProcessId}': {e.Message}");
                }
            }

            var output = OutputFactory.Create(baseOptions.OutputFormat);

            switch (baseOptions.JustificationHandling)
            {
                case Justification.Handle:
                    output.Append("Findings", findings.Select(s => new { s.Rule, s.Asset, s.AssetType, s.Page, s.Stage, s.Message }).ToList());
                    output.Append("Justifications", justifications);
                    break;
                case Justification.Off:
                    output.Append("Findings", findings.Select(s => new { s.Rule, s.Asset, s.AssetType, s.Page, s.Stage, s.Message }).ToList());
                    break;
                case Justification.Only:
                    output.Append("Justifications", justifications);
                    break;
                case Justification.Ignore:
                    output.Append("Findings", findings);
                    break;
            }

            twout.WriteLine(output.ToString());
            return processed;
        }

        public void CheckFile(BpFileOptions fileOptions)
        {
            if (fileOptions is null)
            {
                throw new ArgumentNullException(nameof(fileOptions));
            }

            var assets = fileOptions.ProcessNames.Any() ? fileOptions.ProcessNames : FileProvider.GetAssetsInformation(fileOptions.File, twerror).Select(s => s.Path).ToList();
            if (assets.Any())
            {
                CheckAssets(
                    FileProvider.GetProcesses(fileOptions.File, assets, twerror)
                    .Select(x => new AssetInformation(x.ProcessXml, x.ProcessId, x.IsPublished)), fileOptions);
            }
        }

        public void ShowRules(MinimalOptions baseOptions)
        {
            if (baseOptions is null)
            {
                throw new ArgumentNullException(nameof(baseOptions));
            }

            using var engine = BuildEngine(baseOptions);
            var output = OutputFactory.Create(baseOptions.OutputFormat);
            output.Append("Rules", engine.GetRules());
            twout.WriteLine(output.ToString());
        }

        private void CheckDatabase(string connectionString, DatabaseOptions dbOptions)
        {
            if (dbOptions.ShowTree)
            {
                ShowAssets(connectionString, dbOptions);
            }
            else
            {
                var source = DatabaseProvider.GetProcesses(connectionString, dbOptions.AssetFilter, dbOptions.ProcessNames, dbOptions.AssetIds);
                if (!CheckAssets(source.Select(s => new AssetInformation(s.ProcessXml, s.ProcessId, s.IsPublished)), dbOptions))
                {
                    twerror.WriteLine($"No matching asset found in database.");
                }
            }
        }

        public void CheckNamedDatabase(DatabaseNamedOptions dbOptions)
        {
            if (dbOptions is null)
            {
                throw new ArgumentNullException(nameof(dbOptions));
            }

            var connectionString = ConfigurationManager.ConnectionStrings[dbOptions.ConfigName].ConnectionString;
            if (!string.IsNullOrEmpty(dbOptions.CertThumbprint) || !string.IsNullOrEmpty(dbOptions.CertThumbprintName))
            {
                var certThumbprint = (string.IsNullOrEmpty(dbOptions.CertThumbprint) ?
                    ConfigurationManager.AppSettings[dbOptions.CertThumbprintName]?.Replace(" ", string.Empty) :
                    dbOptions.CertThumbprint.Replace(" ", string.Empty)) ?? throw new InvalidEnumArgumentException("Certificate thumbprint not valid");
                connectionString = DatabaseProvider.DecryptConnectionString(connectionString, certThumbprint, dbOptions.CertStoreName, dbOptions.CertStoreLocation);
            }
            CheckDatabase(connectionString, dbOptions);
        }

        public void CheckConnectionDatabase(DatabaseConnectionOptions dbOptions)
        {
            if (dbOptions is null)
            {
                throw new ArgumentNullException(nameof(dbOptions));
            }

            var connectionString = DatabaseProvider.BuildConnectionString(dbOptions.Server, dbOptions.Database, dbOptions.UserId, dbOptions.Password);
            CheckDatabase(connectionString, dbOptions);
        }

        private AssetInformation? GetProcessFromAutomateC(string assetName, AutomateOptions automateOptions)
        {
            var info = AutomateProvider.GetProcess(automateOptions.AutomateC, assetName, automateOptions.User, automateOptions.Password, automateOptions.ConnectionName, twerror);
            if (info is null)
                return null;
            return new AssetInformation(info.ProcessXml, info.ProcessId, info.IsPublished);
        }

        public void CheckAutomateC(AutomateOptions automateOptions)
        {
            if (automateOptions is null)
            {
                throw new ArgumentNullException(nameof(automateOptions));
            }

            CheckAssets(automateOptions.ProcessNames
                .Select(p => GetProcessFromAutomateC(p, automateOptions)).Where(w => w is not null)!
                , automateOptions);
        }

        private void ShowAssets(string connectionString, BaseOptions baseOptions)
        {
            var output = OutputFactory.Create(baseOptions.OutputFormat);
            output.Append("Tree", DatabaseProvider.GetAssetsInformation(connectionString).ToArray());
            twout.WriteLine(output.ToString());
        }

    }
}
