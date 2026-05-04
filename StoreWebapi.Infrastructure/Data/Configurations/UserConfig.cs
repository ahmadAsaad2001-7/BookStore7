using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Infrastructure.Data.Configurations;

public class userConfiguration : IEntityTypeConfiguration<user>
{
    public void Configure(EntityTypeBuilder<user> builder)
    {
        
        builder.ToTable("users");

       
        builder.HasOne(u => u.gridCell)
            .WithMany(g => g.Users)
            .HasForeignKey(u => u.cellId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(u => u.CouponUsers)
            .WithOne(cu => cu.user)
            .HasForeignKey(cu => cu.userId);
        
       
        builder.HasMany(u => u.ParticipatingVotes)
            .WithOne(vp => vp.user)
            .HasForeignKey(vp => vp.userId);
        
        builder.HasMany(u => u.VotesInitiated)
            .WithOne() 
            .HasForeignKey(v => v.InitiatorId)
            .OnDelete(DeleteBehavior.Restrict);
        
      
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.user)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    
        builder.HasMany(u => u.VendorTransactions)
            .WithOne(t => t.vendor) 
            .HasForeignKey(t => t.vendorId) 
            .OnDelete(DeleteBehavior.Restrict);
        
       
        builder.HasMany(u => u.Transactions)
            .WithOne(t => t.buyer) 
            .HasForeignKey(t => t.userId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
