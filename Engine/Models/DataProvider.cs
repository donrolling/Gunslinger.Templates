using Common.BaseClasses;
using Microsoft.Extensions.Logging;

namespace Gunslinger.Models
{
    public class DataProvider
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool UseLocalDataSource { get; set; }
        public string LocalDataSource { get; set; }
        public string DataSource { get; set; }
    }
}