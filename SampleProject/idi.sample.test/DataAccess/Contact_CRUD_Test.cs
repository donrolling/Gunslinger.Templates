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
    public class Contact_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IContactGateway _contactGateway;

        public Contact_CRUD_Test()
		{
			_contactGateway = ServiceProvider.GetService<IContactGateway>();
		}

        public async Task<Contact> BuildUp()
        {
            //create object
            var contact_Create = _fixture.Build<Contact>().Without(a => a.Id).Create();

            //create entity
            var createResult = await _contactGateway.CreateAsync(contact_Create);
            Assert.IsTrue(createResult.Success);
            return contact_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _contactGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _contactGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task Contact_CRUD_GivenValidValues_Succeeds()
		{

            var contact_Create = await BuildUp();
			var contact_Update = _fixture.Build<Contact>().Without(a => a.Id).Create();
			

			try
			{
				//select object by id to ensure that it was saved to db
				var newContact = await _contactGateway.SelectByIdAsync(contact_Create.Id);
				Assert.IsNotNull(newContact);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				contact_Update.Id = contact_Create.Id;

				//update the item in the database
				var updateResult = await _contactGateway.UpdateAsync(contact_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedContact = await _contactGateway.SelectByIdAsync(contact_Create.Id);
				Assert.IsNotNull(updatedContact);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newContact, updatedContact, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(contact_Create.Id);

			}
		}
    }
}