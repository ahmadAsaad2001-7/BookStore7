using FluentValidation;

namespace StoreWebapi.Application.Features.Admin.CastVote;

public class CastVoteValidator : AbstractValidator<CastVoteCommand>
{
    public CastVoteValidator()
    {
        RuleFor(x=>x.IsApproved).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}