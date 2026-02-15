using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Infrastructure;
using ProductCatalogAPI.Products.CreateProduct;
using ProductCatalogAPI.Products.GetProducts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<CreateProductLogic>();
builder.Services.AddScoped<GetProductsLogic>();

var app = builder.Build();
CreateProductEndpoint.MapEndpoint(app);
GetProductsEndpoint.MapEndpoint(app);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Product Catalog API";
        options.Theme = ScalarTheme.Mars;
    });
}

app.UseHttpsRedirection();
app.Run();