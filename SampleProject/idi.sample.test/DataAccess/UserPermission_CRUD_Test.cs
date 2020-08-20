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
    public class UserPermission_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IUserPermissionGateway _userPermissionGateway;

        public UserPermission_CRUD_Test()
		{
			_userPermissionGateway = ServiceProvider.GetService<IUserPermissionGateway>();
		}

        public async Task<UserPermission> BuildUp(long permissionId,long userId)
        {
            //create object
            var userPermission_Create = _fixture.Build<UserPermission>().Without(a => a.Id).Create();
			userPermission_Create.PermissionId = permissionId;
			userPermission_Create.UserId = userId;

            //create entity
            var createResult = await _userPermissionGateway.CreateAsync(userPermission_Create);
            Assert.IsTrue(createResult.Success);
            return userPermission_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _userPermissionGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _userPermissionGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task UserPermission_CRUD_GivenValidValues_Succeeds()
		{
			var permission_TestClass = new Permission_CRUD_Test();
			var permission_Create = await permission_TestClass.BuildUp();
			var permissionId = permission_Create.Id;
			var user_TestClass = new User_CRUD_Test();
			var user_Create = await user_TestClass.BuildUp();
			var userId = user_Create.Id;

            var userPermission_Create = await BuildUp(permissionId,userId);
			var userPermission_Update = _fixture.Build<UserPermission>().Without(a => a.Id).Create();
			
			userPermission_Update.PermissionId = permissionId;
			userPermission_Update.UserId = userId;

			try
			{
				//select object by id to ensure that it was saved to db
				var newUserPermission = await _userPermissionGateway.SelectByIdAsync(userPermission_Create.Id);
				Assert.IsNotNull(newUserPermission);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				userPermission_Update.Id = userPermission_Create.Id;

				//update the item in the database
				var updateResult = await _userPermissionGateway.UpdateAsync(userPermission_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedUserPermission = await _userPermissionGateway.SelectByIdAsync(userPermission_Create.Id);
				Assert.IsNotNull(updatedUserPermission);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newUserPermission, updatedUserPermission, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(userPermission_Create.Id);

                await permission_TestClass.TearDown(permission_Create.Id);
                await user_TestClass.TearDown(user_Create.Id);
			}
		}
    }
}