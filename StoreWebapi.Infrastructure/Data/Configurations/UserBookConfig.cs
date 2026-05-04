using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Infrastructure.Data.Configurations;

public class UserBookConfig :IEntityTypeConfiguration<UserBook>
{
    public void Configure(EntityTypeBuilder<UserBook> builder)
    {
        builder.HasKey(ub => new { ub.UserId, ub.BookId });
        builder.HasOne(ub => ub.User).
            WithMany(user => user.UserBooks).
            HasForeignKey(ub => ub.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ub=>ub.Book).
            WithMany(b=>b.UserBooks)
            .HasForeignKey(ub => ub.BookId).OnDelete(DeleteBehavior.Cascade);
    }
}