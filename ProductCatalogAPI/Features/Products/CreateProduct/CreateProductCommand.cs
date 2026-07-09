using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Features.Products.CreateProduct;

public record CreateProductCommand(string Name, string Description) : IRequest<Product>;