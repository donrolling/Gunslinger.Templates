using idi.sample.Data.Gateway.Base;
using idi.sample.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace idi.sample.Data.Gateway 
{
	public class PermissionDapperGateway : PermissionDapperBaseGateway, IPermissionGateway
	{	
		public PermissionDapperGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
	}
}