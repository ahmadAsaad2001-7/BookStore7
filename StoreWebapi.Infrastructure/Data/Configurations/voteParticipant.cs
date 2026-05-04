using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;
namespace StoreWebapi.Infrastructure.Data.Configurations;

public class VoteParticipantConfiguration : IEntityTypeConfiguration<voteParticipant>
{
    public void Configure(EntityTypeBuilder<voteParticipant> builder)
    {
        
        builder.HasKey(vp => new { vp.voteId, vp.userId });

        builder.HasOne(vp => vp.vote)
            .WithMany(v => v.Participants)
            .HasForeignKey(vp => vp.voteId);

        builder.HasOne(vp => vp.user)
            .WithMany(u => u.ParticipatingVotes)
            .HasForeignKey(vp => vp.userId);
            
    
    }
}
