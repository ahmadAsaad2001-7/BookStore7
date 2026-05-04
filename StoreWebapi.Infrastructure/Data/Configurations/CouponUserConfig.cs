using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;
namespace StoreWebapi.Infrastructure.Data.Configurations;

public class CouponUserConfiguration : IEntityTypeConfiguration<couponUser>
{   
    public void Configure(EntityTypeBuilder<couponUser> builder)
    {
        builder.HasKey(cu => new { cu.couponId, cu.userId }); // Composite Key

        builder.HasOne(cu => cu.coupon)
            .WithMany(c=>c.couponUsers)
            .HasForeignKey(cu => cu.couponId);

        builder.HasOne(cu => cu.user)
            .WithMany(u => u.CouponUsers)
            .HasForeignKey(cu => cu.userId);
    }
}
