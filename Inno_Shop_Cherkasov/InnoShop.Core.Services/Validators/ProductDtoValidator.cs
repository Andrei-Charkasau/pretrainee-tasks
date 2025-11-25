using FluentValidation;
using InnoShop.Core.DtoModels;

namespace InnoShop.Core.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .Length(2, 100).WithMessage("Product name must be between 2 and 100 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Product name can only contain letters, numbers, spaces, hyphens and underscores");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThan(1000000).WithMessage("Price cannot exceed 1,000,000");
        }
    }
}