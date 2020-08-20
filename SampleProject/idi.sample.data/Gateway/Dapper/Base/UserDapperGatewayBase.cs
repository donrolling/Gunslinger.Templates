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
	public class UserDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<User, long> 
	{
		public UserDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(User user)
		{
			var sql = @"Execute [membership].[User_Insert] 
				 @name
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("name", user.Name);
			_params.Add("isActive", user.IsActive);
			_params.Add("createdBy", user.CreatedBy);
			_params.Add("createdDate", user.CreatedDate);
			_params.Add("modifiedBy", user.ModifiedBy);
			_params.Add("modifiedDate", user.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			user.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(User user) 
		{
			var sql = @"Execute [membership].[User_Update] 
				 @id
				, @name
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, user);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [membership].[User_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<User> SelectByIdAsync(long id)
		{
			var sql = "select * from [membership].[User] where Id = @id";
			return await this.QuerySingleAsync<User>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<User>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [membership].[User]";
			return await this.QueryDynamicAsync<User>(sql, new DynamicParameters(), pageInfo);
		}
	}
}