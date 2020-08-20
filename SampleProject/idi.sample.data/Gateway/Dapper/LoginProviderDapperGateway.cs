using idi.sample.Data.Gateway.Base;
using idi.sample.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace idi.sample.Data.Gateway 
{
	public class LoginProviderDapperGateway : LoginProviderDapperBaseGateway, ILoginProviderGateway
	{	
		public LoginProviderDapperGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
	}
}