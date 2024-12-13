using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        protected readonly IServiceScope _scope;
        protected readonly ISender Sender;
        private readonly IntegrationTestWebAppFactory _factory;


        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        }
        public virtual async Task DisposeAsync()
        {
            _scope.Dispose();
            await _factory.DisposeAsync();
        }
    }
}
