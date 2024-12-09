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
        // Crear el contenedor para SQL Server
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithPassword("YourStrongPassword!")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remover la configuración de DbContext existente
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                // Agregar la nueva configuración de DbContext para el contenedor
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options
                        .UseSqlServer(_dbContainer.GetConnectionString())
                        .UseSnakeCaseNamingConvention();// Obtener la cadena de conexión del contenedor
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
