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
	public class PermissionDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<Permission, long> 
	{
		public PermissionDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(Permission permission)
		{
			var sql = @"Execute [membership].[Permission_Insert] 
				 @name
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("name", permission.Name);
			_params.Add("isActive", permission.IsActive);
			_params.Add("createdBy", permission.CreatedBy);
			_params.Add("createdDate", permission.CreatedDate);
			_params.Add("modifiedBy", permission.ModifiedBy);
			_params.Add("modifiedDate", permission.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			permission.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(Permission permission) 
		{
			var sql = @"Execute [membership].[Permission_Update] 
				 @id
				, @name
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, permission);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [membership].[Permission_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<Permission> SelectByIdAsync(long id)
		{
			var sql = "select * from [membership].[Permission] where Id = @id";
			return await this.QuerySingleAsync<Permission>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<Permission>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [membership].[Permission]";
			return await this.QueryDynamicAsync<Permission>(sql, new DynamicParameters(), pageInfo);
		}
	}
}