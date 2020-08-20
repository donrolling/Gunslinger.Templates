namespace Gunslinger.Models.Settings
{
    public class DataProviderSettings
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string DataSource { get; set; }
        // This allows us to bypass issues with auth on some endpoints
        public bool OpenDataSourceUrlInDefaultBrowser { get; set; } = false;
        // Treat all non-specified properties as nullable
        public bool NonSpecifiedPropertiesAreNullable { get; set; } = false;
    }
}