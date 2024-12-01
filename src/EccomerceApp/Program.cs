using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.PaymentProvider.MercadoPagoProvider;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
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
                    Id = "Ecommerce-Api" } //Tiene que coincidir con el id seteado arriba en la definición
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
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

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
        c.RoutePrefix = string.Empty; // Hace que Swagger esté disponible en la raíz });
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