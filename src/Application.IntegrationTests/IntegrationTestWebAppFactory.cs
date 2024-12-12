using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithPassword("Password123!")
        .WithPortBinding(1435, 1433)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__IntegrationTestConnection")
                                   ?? _dbContainer.GetConnectionString();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                       .UseSnakeCaseNamingConvention();
            });
        });
    }

    // Inicializar el contenedor
    public Task InitializeAsync() => _dbContainer.StartAsync();

    // Liberar los recursos del contenedor
    public new Task DisposeAsync() => _dbContainer.StopAsync();
}
