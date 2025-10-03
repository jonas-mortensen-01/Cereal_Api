using Microsoft.EntityFrameworkCore;
using Cereal_Api.Data;
using Cereal_Api.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddEndpointsApiExplorer();

// Configures the converter for handling Enums contained within url encrypted json strings
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter(
            System.Text.Json.JsonNamingPolicy.CamelCase,
            allowIntegerValues: false
        )
    );
});

// Initial database connection setup
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

var app = builder.Build();

// For testing purposes
// Scalar doesn't allow configuring all requests with the api key for authentication
// so the middleware is disabled if we are in development
if (app.Environment.IsProduction())
{
    app.UseMiddleware<ApiKeyMiddleware>();
}

if (app.Environment.IsDevelopment())
{
    // Maps and creates a scalar api reference
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options.WithTitle("Cereal API Documentation")
            .WithDarkMode(true)
            .WithSidebar(true);
    });
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();