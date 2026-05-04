
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Infrastructure.Data.Configurations;

public class VoteConfiguration : IEntityTypeConfiguration<vote>
{
    public void Configure(EntityTypeBuilder<vote> builder)
    {
        // 1. Primary Key
        builder.HasKey(v => v.Id);

        // 2. Properties
        builder.Property(v => v.subject)
            .IsRequired()
            .HasMaxLength(500);

    
        builder.HasOne<user>() 
            .WithMany() 
            .HasForeignKey(v => v.InitiatorId)
            .OnDelete(DeleteBehavior.Restrict); 
        builder.HasMany(v => v.Participants)
            .WithOne(vp => vp.vote)
            .HasForeignKey(vp => vp.voteId)
            .OnDelete(DeleteBehavior.Cascade); 
        
     
    }
}
    
