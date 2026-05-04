using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Infrastructure.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<comment>
{
    public void Configure(EntityTypeBuilder<comment> builder)
    {
        builder.Property(c => c.text).HasMaxLength(1000);
        
        builder.HasOne(c => c.user)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
            
        builder.HasOne(c => c.book)
            .WithMany(b => b.Comments)
            .HasForeignKey(c => c.bookId);
    }
}
