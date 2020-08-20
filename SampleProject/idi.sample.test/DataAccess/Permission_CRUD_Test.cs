using AutoFixture;
using idi.sample.data.Entities;
using idi.sample.Data.Interfaces;
using idi.sample.test.Base;
using idi.sample.test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace idi.sample.Test.DataAccess {
    [TestClass]
    public class Permission_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IPermissionGateway _permissionGateway;

        public Permission_CRUD_Test()
		{
			_permissionGateway = ServiceProvider.GetService<IPermissionGateway>();
		}

        public async Task<Permission> BuildUp()
        {
            //create object
            var permission_Create = _fixture.Build<Permission>().Without(a => a.Id).Create();

            //create entity
            var createResult = await _permissionGateway.CreateAsync(permission_Create);
            Assert.IsTrue(createResult.Success);
            return permission_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _permissionGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _permissionGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task Permission_CRUD_GivenValidValues_Succeeds()
		{

            var permission_Create = await BuildUp();
			var permission_Update = _fixture.Build<Permission>().Without(a => a.Id).Create();
			

			try
			{
				//select object by id to ensure that it was saved to db
				var newPermission = await _permissionGateway.SelectByIdAsync(permission_Create.Id);
				Assert.IsNotNull(newPermission);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				permission_Update.Id = permission_Create.Id;

				//update the item in the database
				var updateResult = await _permissionGateway.UpdateAsync(permission_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedPermission = await _permissionGateway.SelectByIdAsync(permission_Create.Id);
				Assert.IsNotNull(updatedPermission);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newPermission, updatedPermission, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(permission_Create.Id);

			}
		}
    }
}