using Dapper.SQLGateway.Interfaces;
using idi.sample.data.Entities;

namespace idi.sample.Data.Interfaces 
{
	public interface IContactGateway : IEntityDapperGateway<Contact, long> { }
}