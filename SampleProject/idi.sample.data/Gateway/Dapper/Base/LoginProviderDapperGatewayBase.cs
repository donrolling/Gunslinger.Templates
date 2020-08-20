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
	public class LoginProviderDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<LoginProvider, long> 
	{
		public LoginProviderDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(LoginProvider loginProvider)
		{
			var sql = @"Execute [membership].[LoginProvider_Insert] 
				 @userId
				, @providerName
				, @login
				, @password
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("userId", loginProvider.UserId);
			_params.Add("providerName", loginProvider.ProviderName);
			_params.Add("login", loginProvider.Login);
			_params.Add("password", loginProvider.Password);
			_params.Add("isActive", loginProvider.IsActive);
			_params.Add("createdBy", loginProvider.CreatedBy);
			_params.Add("createdDate", loginProvider.CreatedDate);
			_params.Add("modifiedBy", loginProvider.ModifiedBy);
			_params.Add("modifiedDate", loginProvider.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			loginProvider.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(LoginProvider loginProvider) 
		{
			var sql = @"Execute [membership].[LoginProvider_Update] 
				 @id
				, @userId
				, @providerName
				, @login
				, @password
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, loginProvider);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [membership].[LoginProvider_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<LoginProvider> SelectByIdAsync(long id)
		{
			var sql = "select * from [membership].[LoginProvider] where Id = @id";
			return await this.QuerySingleAsync<LoginProvider>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<LoginProvider>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [membership].[LoginProvider]";
			return await this.QueryDynamicAsync<LoginProvider>(sql, new DynamicParameters(), pageInfo);
		}
	}
}