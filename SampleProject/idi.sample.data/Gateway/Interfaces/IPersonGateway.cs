using Dapper.SQLGateway.Interfaces;
using idi.sample.data.Entities;

namespace idi.sample.Data.Interfaces 
{
	public interface IPersonGateway : IEntityDapperGateway<Person, long> { }
}