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
	public class UserPermissionDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<UserPermission, long> 
	{
		public UserPermissionDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(UserPermission userPermission)
		{
			var sql = @"Execute [membership].[UserPermission_Insert] 
				 @userId
				, @permissionId
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("userId", userPermission.UserId);
			_params.Add("permissionId", userPermission.PermissionId);
			_params.Add("isActive", userPermission.IsActive);
			_params.Add("createdBy", userPermission.CreatedBy);
			_params.Add("createdDate", userPermission.CreatedDate);
			_params.Add("modifiedBy", userPermission.ModifiedBy);
			_params.Add("modifiedDate", userPermission.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			userPermission.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(UserPermission userPermission) 
		{
			var sql = @"Execute [membership].[UserPermission_Update] 
				 @id
				, @userId
				, @permissionId
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, userPermission);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [membership].[UserPermission_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<UserPermission> SelectByIdAsync(long id)
		{
			var sql = "select * from [membership].[UserPermission] where Id = @id";
			return await this.QuerySingleAsync<UserPermission>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<UserPermission>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [membership].[UserPermission]";
			return await this.QueryDynamicAsync<UserPermission>(sql, new DynamicParameters(), pageInfo);
		}
	}
}