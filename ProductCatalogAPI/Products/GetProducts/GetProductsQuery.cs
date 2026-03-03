using MediatR;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Products.GetProducts;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;