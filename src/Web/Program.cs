using Application.Interfaces;
using Application.Services;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.PaymentProvider.MercadoPagoProvider;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Web.Middleware;


var builder = WebApplication.CreateBuilder(args);

#region  Ports

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(44372);
       
    });
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8082); // Puerto para el contenedor
    });
}
#endregion

#region Swagger
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convierte enums a strings
        });

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("Ecommerce-Api", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Ecommerce-Api" } 
                }, new List<string>() }
    });
    setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
#endregion

#region CONNECTION SQL SERVER
var connectionString = builder.Environment.IsEnvironment("Test")
    ? builder.Configuration.GetConnectionString("TestConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

#endregion 

#region REPOSITORIOS
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentsRepository>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region SERVICIOS
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IAuthenticationServiceApi, AuthenticationServiceApi>();
builder.Services.AddScoped<IPaymentNotificationService, PaymentNotificationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped(typeof(IExportService<>), typeof(ExportService<>));
#endregion

#region LOGGING
// Agregar logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});
#endregion

var app = builder.Build();
app.UseDeveloperExceptionPage();
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Test")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseMiddleware<PerformanceMiddleware>(); 
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
public partial class Program { };
