using Common.BaseClasses;
using Gunslinger.Models;
using Microsoft.Extensions.Logging;

namespace Gunslinger.DataProviders
{
    public class DataProviderBase : LoggingWorker
    {
        public DataProvider DataProvider { get; }

        public DataProviderBase(DataProvider dataProvider, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            DataProvider = dataProvider;
        }
    }
}