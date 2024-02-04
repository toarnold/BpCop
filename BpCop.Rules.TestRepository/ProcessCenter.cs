using BpCop.DataProviders;
using System;
using System.Linq;

namespace BpCop.Rules.TestRepository
{
    public static class ProcessCenter
    {
#if USE_DATABASE
        public static string LoadAsset(string name)
        {
            var connectionString = DatabaseProvider.BuildConnectionString("localhost", "BluePrism", "sa", "password");

            return DatabaseProvider.GetProcesses(connectionString, string.Empty, new[] { name }, null).Single().ProcessXml;
        }
#else
        public static string LoadAsset(string name)
        {
            return FileProvider.GetProcesses("..\\..\\..\\.\\.\\..\\BpCop - Tests.bprelease", new[] { name }, Console.Out).Single().ProcessXml;
        }
#endif
    }
}
