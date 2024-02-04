using BpCop.DataProviders.Dto;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BpCop.DataProviders
{
    public static class AutomateProvider
    {
        public static ProcessInformation? GetProcess(string automatePath, string assetName, string userName, string password, string connectionName, TextWriter twerror)
        {
            if (twerror is null)
            {
                throw new ArgumentNullException(nameof(twerror));
            }

            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            try
            {
                using var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = automatePath;
                p.StartInfo.WorkingDirectory = tempPath;
                var arguments = new StringBuilder($"/export \"{assetName}\"");

                if (string.IsNullOrEmpty(userName))
                {
                    arguments.Append(" /sso");
                }
                else
                {
                    arguments.Append($" /user \"{userName}\" \"{password}\"");
                }
                if (!string.IsNullOrEmpty(connectionName))
                {
                    arguments.Append($" /dbconname \"{connectionName}\"");
                }
                p.StartInfo.Arguments = arguments.ToString();
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (p.ExitCode == 0)
                {
                    var filename = Directory.GetFiles(tempPath).Single(f => f.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
                    return FileProvider.GetProcesses(filename, Enumerable.Empty<string>(), twerror).Single();
                }
                else
                {
                    twerror.WriteLine(output);
                    return null;
                }
            }
            catch (Exception e)
            {
                twerror.WriteLine(e.Message);
                return null;
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }
    }
}
