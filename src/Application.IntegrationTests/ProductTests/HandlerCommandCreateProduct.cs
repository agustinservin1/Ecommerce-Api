using MediatR;
using Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace IntegrationTests.ProductTests
{
    public class HandlerCommandCreateProduct : IRequestHandler<CommandCreateProduct, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public HandlerCommandCreateProduct(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CommandCreateProduct request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Stock = request.Stock,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Image = request.Image
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}