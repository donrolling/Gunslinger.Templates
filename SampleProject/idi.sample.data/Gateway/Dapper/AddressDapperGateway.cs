using idi.sample.Data.Gateway.Base;
using idi.sample.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace idi.sample.Data.Gateway 
{
	public class AddressDapperGateway : AddressDapperBaseGateway, IAddressGateway
	{	
		public AddressDapperGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
	}
}