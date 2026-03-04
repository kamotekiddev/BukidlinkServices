using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Product>;