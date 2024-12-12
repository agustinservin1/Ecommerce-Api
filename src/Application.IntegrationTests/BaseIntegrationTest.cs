using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        protected readonly IServiceScope _scope;
        protected readonly ISender Sender;
        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();  
                 
        }
        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
