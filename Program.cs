using Microsoft.EntityFrameworkCore;    // Para UseSqlServer y AddDbContext
using Scalar.AspNetCore;                // Importa las extensiones de Scalar para poder usar MapScalarApiReference
using VehiculosApi.Data;
using VehiculosApi.Repositories;                // Para encontrar VehiculosDbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Registro del DbContext en el contenedor de inyección de dependencias
builder.Services.AddDbContext<VehiculosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MiCadenaDeConexion")));

builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();  // Esto le dice a .NET: “cuando alguien pida un IVehiculoRepository, entrégale un VehiculoRepository”.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                   // Genera el documento OpenAPI en /openapi/v1.json
    app.MapScalarApiReference();        // Crea la página visual que lee ese documento
}    

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
