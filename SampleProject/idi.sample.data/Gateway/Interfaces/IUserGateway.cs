using Dapper.SQLGateway.Interfaces;
using idi.sample.data.Entities;

namespace idi.sample.Data.Interfaces 
{
	public interface IUserGateway : IEntityDapperGateway<User, long> { }
}