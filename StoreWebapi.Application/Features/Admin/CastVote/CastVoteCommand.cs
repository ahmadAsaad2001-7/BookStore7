using MediatR;

namespace StoreWebapi.Application.Features.Admin.CastVote;

public record CastVoteCommand(Guid VoteId, Guid AdminId, bool IsApproved) : IRequest<Result<CastVoteResponse>>;
