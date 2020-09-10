using Gunslinger.Models.Settings;

namespace Gunslinger.Interfaces
{
    public interface IDataProviderFactory
    {
        IDataProvider Create(dynamic dataProviderSettings);

        IDataProvider Get(string name);
    }
}