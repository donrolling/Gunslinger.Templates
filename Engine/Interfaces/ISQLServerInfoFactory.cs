using Gunslinger.Models;
using Gunslinger.Models.SQL;

namespace Gunslinger.Interfaces
{
    public interface ISQLServerInfoFactory
    {
        SQLServerInfo Create(DataProvider dataProvider);
    }
}