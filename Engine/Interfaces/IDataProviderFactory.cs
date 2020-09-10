using Gunslinger.Models;

namespace Gunslinger.Interfaces
{
    public interface IDataProviderFactory
    {
        IDataProvider Create(DataProvider dataProvider);
        IDataProvider Get(string name);
    }
}