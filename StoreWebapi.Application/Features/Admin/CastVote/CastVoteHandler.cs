using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Admin.CastVote;

public class CastVoteHandler(IRepository repo, UserManager<user> userManager) 
    : IRequestHandler<CastVoteCommand, Result<CastVoteResponse>>
{
    public async Task<Result<CastVoteResponse>> Handle(CastVoteCommand request, CancellationToken cancellationToken)
    {
        
        var vote = await repo.FindById<vote>(request.VoteId);
        if (vote == null || vote.IsResolved == true) 
            return Result.Failure<CastVoteResponse>("Vote not found or already finished.");

        
        var alreadyVoted = await repo.AnyAsync<voteParticipant>(p => 
            p.voteId == request.VoteId && p.userId == request.AdminId);
        
        if (alreadyVoted)
            return Result.Failure<CastVoteResponse>("You have already voted on this decision.");

      
        if (request.IsApproved) vote.Approval++;
        else vote.disApprove++;

        repo.Add(new voteParticipant { voteId = request.VoteId, userId = request.AdminId });

        
        bool finalized = false;
        if (vote.Approval >= 2)
        {
            finalized = true;
            vote.IsResolved = true; 
            await ExecuteVoteAction(vote); 
        }

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success(new CastVoteResponse { 
            IsFinalized = finalized, 
            CurrentApprovals = vote.Approval,
            Message = finalized ? "Threshold reached. Action executed." : "Vote recorded."
        });
    }

    private async Task ExecuteVoteAction(vote vote)
    {
        if (vote.subject.StartsWith("RoleChange:Vendor"))
        {
            var user = await userManager.FindByIdAsync(vote.InitiatorId.ToString());
            if (user != null)
            {
                await userManager.AddToRoleAsync(user, "Vendor");
            }
        }
     
    }
}