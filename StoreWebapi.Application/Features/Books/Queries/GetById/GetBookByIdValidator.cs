using FluentValidation;
using StoreWebapi.Application.Features.Books.SearchBooks;

namespace StoreWebapi.Application.Features.Books.Queries;

public class GetBookByIdValidator : AbstractValidator<GetBookByIdQuery>
{
    public GetBookByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Book ID is required.");

    }
}