using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Common.Exceptions;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.Domain.ValueObjects;
using ProductCatalogAPI.Infrastructure;

namespace ProductCatalogAPI.ProductVariants.CreateProductVariant;

public class CreateProductVariantHandler(AppDbContext dbContext, RabbitMqPublisher publisher)
    : IRequestHandler<CreateProductVariantCommand, ProductVariant>
{
    public async Task<ProductVariant> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var variant =
            await dbContext.ProductVariants.FirstOrDefaultAsync(
                pv => pv.Name == request.Name && pv.ProductId == request.ProductId, cancellationToken);

        if (variant is not null)
            throw new ProductVariantAlreadyExistException(
                $"Product variant with the given name: {request.Name} and productId: {request.ProductId} already exists in the system.");

        variant = new ProductVariant
        {
            Name = request.Name,
            Price = new Money(request.Price, "PHP"),
            ProductId = request.ProductId,
            Sku = Sku.Create(request.SkuValue)
        };

        dbContext.ProductVariants.Add(variant);
        await dbContext.SaveChangesAsync(cancellationToken);

        await publisher.PublishAsync(
            exchange: "product.events",
            routingKey: "variant.created",
            message: new
            {
                VariantId = variant.Id,
                ProductId = variant.ProductId,
                Name = variant.Name,
                Sku = variant.Sku.Value,
                Price = variant.Price.Value
            });


        return variant;
    }
}