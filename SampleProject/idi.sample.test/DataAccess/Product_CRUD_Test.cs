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
    public class Product_CRUD_Test : DataAccessTestBase
	{
        private readonly Fixture _fixture = new Fixture();
		private readonly IProductGateway _productGateway;

        public Product_CRUD_Test()
		{
			_productGateway = ServiceProvider.GetService<IProductGateway>();
		}

        public async Task<Product> BuildUp()
        {
            //create object
            var product_Create = _fixture.Build<Product>().Without(a => a.Id).Create();

            //create entity
            var createResult = await _productGateway.CreateAsync(product_Create);
            Assert.IsTrue(createResult.Success);
            return product_Create;
        }

        public async Task TearDown(long id)
        {
            // delete the item in the database
            var deleteResult = await _productGateway.DeleteAsync(id);
            Assert.IsTrue(deleteResult.Success);

            // verify that the item was deleted
            var deleteConfirmContact = await _productGateway.SelectByIdAsync(id);
            Assert.IsNull(deleteConfirmContact);
        }

		[TestMethod]
		public async Task Product_CRUD_GivenValidValues_Succeeds()
		{

            var product_Create = await BuildUp();
			var product_Update = _fixture.Build<Product>().Without(a => a.Id).Create();
			

			try
			{
				//select object by id to ensure that it was saved to db
				var newProduct = await _productGateway.SelectByIdAsync(product_Create.Id);
				Assert.IsNotNull(newProduct);
				
				// set the update object to have the same Id as the create object so we can easily assign new values
				product_Update.Id = product_Create.Id;

				//update the item in the database
				var updateResult = await _productGateway.UpdateAsync(product_Update);
				Assert.IsTrue(updateResult.Success);

				//verify that the data in the newly updated object is not the same as it was previously.
				var updatedProduct = await _productGateway.SelectByIdAsync(product_Create.Id);
				Assert.IsNotNull(updatedProduct);
				
                // assert that changes were made
				// ignoring id because we know it is the same
                var areEqual = TestComparison.DeepCompare(newProduct, updatedProduct, new List<string> { "Id" });
                Assert.IsTrue(areEqual.Failure, areEqual.Message);
			}
			finally 
			{
                await TearDown(product_Create.Id);

			}
		}
    }
}