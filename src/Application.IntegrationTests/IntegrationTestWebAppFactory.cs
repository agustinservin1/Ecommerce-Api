using Infrastructure.Data;
using IntegrationTests.ProductTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace IntegrationTests
{

    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer;
        public IntegrationTestWebAppFactory()
        {
            _dbContainer = new MsSqlBuilder().Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var connectionString = _dbContainer.GetConnectionString();
                Console.WriteLine(_dbContainer.GetConnectionString());

                services.AddDbContext<ApplicationDbContext>((options) =>
                {
                    options.UseSqlServer(connectionString)
                    .UseSnakeCaseNamingConvention();
                });
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CommandCreateProduct).Assembly));
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
        }
        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}