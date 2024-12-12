using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql; 

namespace Application.IntegrationTests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        //contenedor para sqlserver
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithPassword("Password123!")
            .WithPortBinding(1435, 1433) 
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                // nueva config de DbContext para el contenedor
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options
                        .UseSqlServer(_dbContainer.GetConnectionString())
                        .UseSnakeCaseNamingConvention();
                });
            });
        }

        // Inicializar el contenedor
        public Task InitializeAsync()
        {
            return  _dbContainer.StartAsync();
        }

        // Liberar los recursos del contenedor
        public new Task DisposeAsync()
        {
           return _dbContainer.StopAsync();
        }
    }
}
