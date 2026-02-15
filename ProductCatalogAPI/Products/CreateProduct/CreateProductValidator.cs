using FluentValidation;

namespace ProductCatalogAPI.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("This field is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("This field is required.");
    }
}