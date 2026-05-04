using FluentValidation;

namespace StoreWebapi.Application.Features.Books.Queries.GetBooksByCategory;

public class GetBooksByCategoryValidator : AbstractValidator<GetBooksByCategoryQuery>
{
    public GetBooksByCategoryValidator()
    {
        RuleFor(x=>x.GenresList).NotEmpty().WithMessage("{Genres is required.");
    }
}