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
    public class Address_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IAddressGateway _addressGateway;

        public Address_CRUD_Test()
		{
			_addressGateway = ServiceProvider.GetService<IAddressGateway>();
		}

        public async Task<Address> BuildUp()
        {
            //create object
            var address_Create = _fixture.Build<Address>().Without(a => a.Id).Create();

            //create entity
            var createResult = await _addressGateway.CreateAsync(address_Create);
            Assert.IsTrue(createResult.Success);
            return address_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _addressGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _addressGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task Address_CRUD_GivenValidValues_Succeeds()
		{

            var address_Create = await BuildUp();
			var address_Update = _fixture.Build<Address>().Without(a => a.Id).Create();
			

			try
			{
				//select object by id to ensure that it was saved to db
				var newAddress = await _addressGateway.SelectByIdAsync(address_Create.Id);
				Assert.IsNotNull(newAddress);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				address_Update.Id = address_Create.Id;

				//update the item in the database
				var updateResult = await _addressGateway.UpdateAsync(address_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedAddress = await _addressGateway.SelectByIdAsync(address_Create.Id);
				Assert.IsNotNull(updatedAddress);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newAddress, updatedAddress, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(address_Create.Id);

			}
		}
    }
}