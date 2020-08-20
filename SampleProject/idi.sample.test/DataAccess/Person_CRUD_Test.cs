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
    public class Person_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IPersonGateway _personGateway;

        public Person_CRUD_Test()
		{
			_personGateway = ServiceProvider.GetService<IPersonGateway>();
		}

        public async Task<Person> BuildUp()
        {
            //create object
            var person_Create = _fixture.Build<Person>().Without(a => a.Id).Create();

            //create entity
            var createResult = await _personGateway.CreateAsync(person_Create);
            Assert.IsTrue(createResult.Success);
            return person_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _personGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _personGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task Person_CRUD_GivenValidValues_Succeeds()
		{

            var person_Create = await BuildUp();
			var person_Update = _fixture.Build<Person>().Without(a => a.Id).Create();
			

			try
			{
				//select object by id to ensure that it was saved to db
				var newPerson = await _personGateway.SelectByIdAsync(person_Create.Id);
				Assert.IsNotNull(newPerson);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				person_Update.Id = person_Create.Id;

				//update the item in the database
				var updateResult = await _personGateway.UpdateAsync(person_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedPerson = await _personGateway.SelectByIdAsync(person_Create.Id);
				Assert.IsNotNull(updatedPerson);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newPerson, updatedPerson, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(person_Create.Id);

			}
		}
    }
}