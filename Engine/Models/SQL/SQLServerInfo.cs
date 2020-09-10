using Gunslinger.Models.Settings;
using Microsoft.SqlServer.Management.Smo;

namespace Gunslinger.Models.SQL
{
    public class SQLServerInfo
    {
        public string Name => DataProvider.Name;

        public string DatabaseName { get; set; }
        public DataProviderSettings DataProvider { get; set; }
        public string ServerName { get; set; }
        public Server Server { get; set; }
        public Database Database { get; set; }
    }
}