using Gunslinger.Models;
using Gunslinger.Models.Settings;
using Gunslinger.Models.SQL;

namespace Gunslinger.Interfaces
{
    public interface ISQLServerInfoFactory
    {
        SQLServerInfo Create(DataProviderSettings dataProviderSettings);
    }
}