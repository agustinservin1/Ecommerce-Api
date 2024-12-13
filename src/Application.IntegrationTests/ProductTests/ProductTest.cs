using IntegrationTests.ProductTests;
using Infrastructure.Data;
using IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.ProductTests
{
    public class ProductTest : BaseIntegrationTest
    {
        public ProductTest(IntegrationTestWebAppFactory factory) : base(factory) { }

        [Fact]
        public async Task Should_Create_Product()
        {
            // Arrange
            var createProductRequest = new CommandCreateProduct
            {
                Name = "Test Product",
                Description = "This is a test product",
                Stock = 100,
                Price = 25,
                CategoryId = 1,
                Image = "image_url"
            };

            // Act
            var productId = await Sender.Send(createProductRequest);

            // Assert
            using var scope = _scope.ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");

            Assert.NotNull(product);
            Assert.Equal("Test Product", product.Name);
            Assert.Equal(25, product.Price);
            Assert.Equal(100, product.Stock);
        }

    }
}
