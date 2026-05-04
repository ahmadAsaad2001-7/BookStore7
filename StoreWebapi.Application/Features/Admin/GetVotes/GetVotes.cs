using MediatR;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Admin.GetVotes;

public record GetVotesQuery(Guid AdminId) : IRequest<Result<List<GetVotesResponse>>>;

public class GetVotesResponse
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public int ApprovalCount { get; set; }
    public int DisapprovalCount { get; set; }
    public bool IsResolved { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    public bool HasVoted { get; set; } 
    public bool IsExpired => DateTime.UtcNow > ExpiryDate;
    public bool CanVote => !IsResolved && !IsExpired && !HasVoted;
}
public class GetVotesHandler(IRepository repo) : IRequestHandler<GetVotesQuery, Result<List<GetVotesResponse>>>
{
    public async Task<Result<List<GetVotesResponse>>> Handle(GetVotesQuery request, CancellationToken cancellationToken)
    {
        var votes = await repo.FindAll<vote>();

      
        var adminParticipations = await repo.FindAll<voteParticipant>(vp => vp.userId == request.AdminId);

      
        var votedIds = adminParticipations.Select(p => p.voteId).ToHashSet();

        var response = votes.Select(v => new GetVotesResponse
        {
            Id = v.Id,
            Subject = v.subject,
            ApprovalCount = v.Approval,
            DisapprovalCount = v.disApprove,
            IsResolved = v.IsResolved,
            ExpiryDate = v.expiryDate,
            
        
            HasVoted = votedIds.Contains(v.Id) 
        }).ToList();

        return Result.Success(response);
    }
}