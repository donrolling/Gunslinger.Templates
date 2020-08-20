using Dapper;
using Dapper.SQLGateway;
using Dapper.SQLGateway.Interfaces;
using Dapper.SQLGateway.Models;
using idi.sample.data.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Data;

namespace idi.sample.Data.Gateway.Base 
{
	public class PersonDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<Person, long> 
	{
		public PersonDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(Person person)
		{
			var sql = @"Execute [dbo].[Person_Insert] 
				 @firstName
				, @lastName
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("firstName", person.FirstName);
			_params.Add("lastName", person.LastName);
			_params.Add("isActive", person.IsActive);
			_params.Add("createdBy", person.CreatedBy);
			_params.Add("createdDate", person.CreatedDate);
			_params.Add("modifiedBy", person.ModifiedBy);
			_params.Add("modifiedDate", person.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			person.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(Person person) 
		{
			var sql = @"Execute [dbo].[Person_Update] 
				 @id
				, @firstName
				, @lastName
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, person);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [dbo].[Person_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<Person> SelectByIdAsync(long id)
		{
			var sql = "select * from [dbo].[Person] where Id = @id";
			return await this.QuerySingleAsync<Person>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<Person>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [dbo].[Person]";
			return await this.QueryDynamicAsync<Person>(sql, new DynamicParameters(), pageInfo);
		}
	}
}