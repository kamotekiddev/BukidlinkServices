using FluentValidation;

namespace ProductCatalogAPI.Features.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("This field is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("This field is required.");
    }
}