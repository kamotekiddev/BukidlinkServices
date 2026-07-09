using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Features.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Product>;