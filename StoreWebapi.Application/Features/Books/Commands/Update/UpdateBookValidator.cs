using FluentValidation;

namespace StoreWebapi.Application.Features.Books.Commands.Update;

public class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookValidator()
    {
        RuleFor(x=>x.isbn).NotEmpty().WithMessage("ISBN is required");
        RuleFor(x=>x.name).NotEmpty().WithMessage("Name is required");
        RuleFor(x=>x.imageUrl).NotEmpty().WithMessage("ImageUrl is required");
        RuleFor(x=>x.description).NotEmpty().WithMessage("Description is required");
        RuleFor(x=>x.price).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x=>x.author).NotEmpty().WithMessage("Author is required");
        RuleFor(x=>x.genres).NotEmpty().WithMessage("Genre is required");
        
    }
}