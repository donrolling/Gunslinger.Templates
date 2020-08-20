using idi.sample.Data.Gateway;
using idi.sample.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace idi.sample.test.Injection {
    public static class DataAccess
    {
        public static void AddServices(string connectionString, ILoggerFactory loggerFactory, ServiceCollection services)
        {
            services.AddTransient<IAddressGateway, AddressDapperGateway>(a => new AddressDapperGateway(connectionString, loggerFactory));
            services.AddTransient<IContactGateway, ContactDapperGateway>(a => new ContactDapperGateway(connectionString, loggerFactory));
            services.AddTransient<IPersonGateway, PersonDapperGateway>(a => new PersonDapperGateway(connectionString, loggerFactory));
            services.AddTransient<IProductGateway, ProductDapperGateway>(a => new ProductDapperGateway(connectionString, loggerFactory));
            services.AddTransient<ILoginProviderGateway, LoginProviderDapperGateway>(a => new LoginProviderDapperGateway(connectionString, loggerFactory));
            services.AddTransient<IPermissionGateway, PermissionDapperGateway>(a => new PermissionDapperGateway(connectionString, loggerFactory));
            services.AddTransient<IUserGateway, UserDapperGateway>(a => new UserDapperGateway(connectionString, loggerFactory));
            services.AddTransient<IUserPermissionGateway, UserPermissionDapperGateway>(a => new UserPermissionDapperGateway(connectionString, loggerFactory));
        }
    }
}