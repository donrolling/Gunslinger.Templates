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
	public class AddressDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<Address, long> 
	{
		public AddressDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(Address address)
		{
			var sql = @"Execute [dbo].[Address_Insert] 
				 @address1
				, @address2
				, @city
				, @state
				, @zip
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("address1", address.Address1);
			_params.Add("address2", address.Address2);
			_params.Add("city", address.City);
			_params.Add("state", address.State);
			_params.Add("zip", address.Zip);
			_params.Add("isActive", address.IsActive);
			_params.Add("createdBy", address.CreatedBy);
			_params.Add("createdDate", address.CreatedDate);
			_params.Add("modifiedBy", address.ModifiedBy);
			_params.Add("modifiedDate", address.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			address.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(Address address) 
		{
			var sql = @"Execute [dbo].[Address_Update] 
				 @id
				, @address1
				, @address2
				, @city
				, @state
				, @zip
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, address);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [dbo].[Address_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<Address> SelectByIdAsync(long id)
		{
			var sql = "select * from [dbo].[Address] where Id = @id";
			return await this.QuerySingleAsync<Address>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<Address>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [dbo].[Address]";
			return await this.QueryDynamicAsync<Address>(sql, new DynamicParameters(), pageInfo);
		}
	}
}