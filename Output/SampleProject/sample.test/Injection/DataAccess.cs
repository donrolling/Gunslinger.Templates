using sample.Data.Gateway;
using sample.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace sample.test.Injection {
    public static class DataAccess
    {
        public static void AddServices(string connectionString, ILoggerFactory loggerFactory, ServiceCollection services)
        {
        }
    }
}