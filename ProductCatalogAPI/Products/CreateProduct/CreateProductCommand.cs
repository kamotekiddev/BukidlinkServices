using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name, string Description) : IRequest<Product>;