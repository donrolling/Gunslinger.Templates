using Dapper.SQLGateway.Interfaces;
using idi.sample.data.Entities;

namespace idi.sample.Data.Interfaces 
{
	public interface IUserPermissionGateway : IEntityDapperGateway<UserPermission, long> { }
}