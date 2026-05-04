using FluentValidation;

namespace StoreWebapi.Application.Features.Vendor.Sell;

public class SellBookValidator : AbstractValidator<SellBookCommand>
{
    public SellBookValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl is required");
        RuleFor(x => x.Isbn).NotEmpty().WithMessage("Isbn is required");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        RuleFor(x=>x.Genres).NotEmpty().WithMessage("Genres is required");
        
    }
}