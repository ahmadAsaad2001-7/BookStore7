using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Infrastructure.Data.Configurations;

public class CouponsConfiguration : IEntityTypeConfiguration<coupons>
{
    public void Configure(EntityTypeBuilder<coupons> builder)
    {
        builder.HasKey(c => c.couponId);
        
        builder.Property(c => c.Discount_percentage).HasPrecision(5, 2);
        
        builder.HasMany(c => c.couponUsers)
       .WithOne(cu => cu.coupon)
       .HasForeignKey(cu => cu.couponId);
        
      
        builder.Ignore(c => c.IsExpired); 
    }
}

