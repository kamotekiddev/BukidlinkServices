using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Features.Products.GetProducts;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;