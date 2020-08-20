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
	public class ContactDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<Contact, long> 
	{
		public ContactDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(Contact contact)
		{
			var sql = @"Execute [dbo].[Contact_Insert] 
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
			_params.Add("firstName", contact.FirstName);
			_params.Add("lastName", contact.LastName);
			_params.Add("isActive", contact.IsActive);
			_params.Add("createdBy", contact.CreatedBy);
			_params.Add("createdDate", contact.CreatedDate);
			_params.Add("modifiedBy", contact.ModifiedBy);
			_params.Add("modifiedDate", contact.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			contact.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(Contact contact) 
		{
			var sql = @"Execute [dbo].[Contact_Update] 
				 @id
				, @firstName
				, @lastName
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, contact);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [dbo].[Contact_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<Contact> SelectByIdAsync(long id)
		{
			var sql = "select * from [dbo].[Contact] where Id = @id";
			return await this.QuerySingleAsync<Contact>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<Contact>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [dbo].[Contact]";
			return await this.QueryDynamicAsync<Contact>(sql, new DynamicParameters(), pageInfo);
		}
	}
}