using Application.Interfaces;
using Application.Services;
using DocumentFormat.OpenXml.Office2021.MipLabelMetaData;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.PaymentProvider.MercadoPagoProvider;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Swagger
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convierte enums a strings
        });
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("tpi-progIII", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Ac� pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Ecommerce-Api" } //Tiene que coincidir con el id seteado arriba en la definici�n
                }, new List<string>() }
    });
    setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
#endregion


#region CONNECTION SQL SERVER

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



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
#region LOGGING CONFIGURATION
builder.Logging.ClearProviders(); 
builder.Logging.AddConsole();
builder.Logging.AddDebug();
#endregion

var app = builder.Build();
app.UseDeveloperExceptionPage();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();//ver
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Hace que Swagger est� disponible en la ra�z });
    });
}
// Registrar el middleware de manejo de excepciones globales
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseCors("CorsPolicy");
app.Run();
#endregion