using FluentValidation;

namespace StoreWebapi.Application.Features.Books.Commands.Create;

public class CreateValidator :AbstractValidator<CreateCommand>
{
    public CreateValidator()
    {
        RuleFor(x=>x.author).NotEmpty().WithMessage("Author is required");
        RuleFor(x=>x.description).NotEmpty().WithMessage("Description is required");
        RuleFor(x=>x.isbn).NotEmpty().WithMessage("ISBN is required");
        RuleFor(x=>x.genres).NotEmpty().WithMessage("Genres is required");
        RuleFor(x=>x.imageUrl).NotEmpty().WithMessage("ImageUrl is required");
        RuleFor(x=>x.price).NotEmpty().WithMessage("Price is required");
        RuleFor(x=>x.name).NotEmpty().WithMessage("Name is required");
    }
} 