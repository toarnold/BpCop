using CommandLine;

namespace BpCop.Console.Common.Parameters
{
    [Verb("check-named-db", HelpText = "Load assets from a Blue Prism database stored in the app config.")]
    public sealed class DatabaseNamedOptions : DatabaseOptions
    {
        [Option('n', "name", Required = true, HelpText = "Database configuration name.")]
        public string ConfigName { get; set; } = string.Empty;

        [Option("cert-tumbprint-name", Default = "DefaultCertificateThumbprint", Group = "Certificate", HelpText = "Certificate thumbprint app settings key name to decrypt connection string.")]
        public string CertThumbprintName { get; set; } = string.Empty;

        [Option("cert-tumbprint", Default = "", Group = "Certificate", HelpText = "Certificate thumbprint to decrypt connection string.")]
        public string CertThumbprint { get; set; } = string.Empty;

        [Option("cert-store-name", Default = "My", Group = "Certificate", HelpText = "Certificate's store name (default to 'My').")]
        public string CertStoreName { get; set; } = string.Empty;

        [Option("cert-store-location", Default = "CurrentUser", Group = "Certificate", HelpText = "Certificate's store location (default to 'CurrentUser').")]
        public string CertStoreLocation { get; set; } = string.Empty;
    }
}
