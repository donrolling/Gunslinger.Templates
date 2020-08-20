namespace Gunslinger.Models.Settings
{
    public class SwaggerDataProviderSettings : DataProviderSettings
    {
        public bool UseLocalDataSource { get; set; }
        public string LocalDataSource { get; set; }
    }
}