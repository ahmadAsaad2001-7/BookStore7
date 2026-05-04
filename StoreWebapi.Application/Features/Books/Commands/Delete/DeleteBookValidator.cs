using System.Data;
using FluentValidation;

namespace StoreWebapi.Application.Features.Books.Commands.Delete;

public class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookValidator()
    {
        RuleFor(x=>x.Id).NotEmpty().WithMessage("Id is required");
    }
}