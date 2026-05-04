using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreWebapi.Domain.Domain;
namespace StoreWebapi.Infrastructure.Data.Configurations;

public class GridCellConfiguration : IEntityTypeConfiguration<gridCell>
{
    public void Configure(EntityTypeBuilder<gridCell> builder)
    {
        builder.HasKey(g => g.cellId); 
        builder.HasMany(g => g.Users)
            .WithOne(u => u.gridCell)
            .HasForeignKey(u => u.cellId);
    }
}
