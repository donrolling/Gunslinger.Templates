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
    public class User_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IUserGateway _userGateway;

        public User_CRUD_Test()
		{
			_userGateway = ServiceProvider.GetService<IUserGateway>();
		}

        public async Task<User> BuildUp()
        {
            //create object
            var user_Create = _fixture.Build<User>().Without(a => a.Id).Create();

            //create entity
            var createResult = await _userGateway.CreateAsync(user_Create);
            Assert.IsTrue(createResult.Success);
            return user_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _userGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _userGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task User_CRUD_GivenValidValues_Succeeds()
		{

            var user_Create = await BuildUp();
			var user_Update = _fixture.Build<User>().Without(a => a.Id).Create();
			

			try
			{
				//select object by id to ensure that it was saved to db
				var newUser = await _userGateway.SelectByIdAsync(user_Create.Id);
				Assert.IsNotNull(newUser);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				user_Update.Id = user_Create.Id;

				//update the item in the database
				var updateResult = await _userGateway.UpdateAsync(user_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedUser = await _userGateway.SelectByIdAsync(user_Create.Id);
				Assert.IsNotNull(updatedUser);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newUser, updatedUser, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(user_Create.Id);

			}
		}
    }
}