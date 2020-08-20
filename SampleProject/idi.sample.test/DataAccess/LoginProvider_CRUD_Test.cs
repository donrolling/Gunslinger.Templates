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
    public class LoginProvider_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly ILoginProviderGateway _loginProviderGateway;

        public LoginProvider_CRUD_Test()
		{
			_loginProviderGateway = ServiceProvider.GetService<ILoginProviderGateway>();
		}

        public async Task<LoginProvider> BuildUp(long userId)
        {
            //create object
            var loginProvider_Create = _fixture.Build<LoginProvider>().Without(a => a.Id).Create();
			loginProvider_Create.UserId = userId;

            //create entity
            var createResult = await _loginProviderGateway.CreateAsync(loginProvider_Create);
            Assert.IsTrue(createResult.Success);
            return loginProvider_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _loginProviderGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _loginProviderGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task LoginProvider_CRUD_GivenValidValues_Succeeds()
		{
			var user_TestClass = new User_CRUD_Test();
			var user_Create = await user_TestClass.BuildUp();
			var userId = user_Create.Id;

            var loginProvider_Create = await BuildUp(userId);
			var loginProvider_Update = _fixture.Build<LoginProvider>().Without(a => a.Id).Create();
			
			loginProvider_Update.UserId = userId;

			try
			{
				//select object by id to ensure that it was saved to db
				var newLoginProvider = await _loginProviderGateway.SelectByIdAsync(loginProvider_Create.Id);
				Assert.IsNotNull(newLoginProvider);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				loginProvider_Update.Id = loginProvider_Create.Id;

				//update the item in the database
				var updateResult = await _loginProviderGateway.UpdateAsync(loginProvider_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedLoginProvider = await _loginProviderGateway.SelectByIdAsync(loginProvider_Create.Id);
				Assert.IsNotNull(updatedLoginProvider);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newLoginProvider, updatedLoginProvider, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(loginProvider_Create.Id);

                await user_TestClass.TearDown(user_Create.Id);
			}
		}
    }
}