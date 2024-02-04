using CommandLine;

namespace BpCop.Console.Common.Parameters
{
    [Verb("check-db", HelpText = "Loads assets from the Blue Prism database.")]
    public sealed class DatabaseConnectionOptions : DatabaseOptions
    {
        [Option('s', "server", Required = true, HelpText = "Database server name or ip adress.")]
        public string Server { get; set; } = string.Empty;

        [Option('n', "name", Required = true, HelpText = "Database name.")]
        public string Database { get; set; } = string.Empty;

        [Option('u', "user", HelpText = "Database user name (leave blank for integrated security).")]
        public string UserId { get; set; } = string.Empty;

        [Option('p', "password", HelpText = "Database user password.")]
        public string Password { get; set; } = string.Empty;
    }
}
