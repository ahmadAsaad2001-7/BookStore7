using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Infrastructure.Data.Configurations;


public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.id);
        
        builder.Property(b => b.name).IsRequired().HasMaxLength(200);
        
        builder.Property(b => b.price)
            .HasPrecision(18, 2);
            builder.HasMany(b => b.Transactions)
            .WithOne(t => t.book)
            .HasForeignKey(t => t.bookId);
        builder.HasOne(b=>b.user).WithMany(u=>u.BooksRelated).HasForeignKey(b=>b.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(b => b.Version)
                .IsRequired()
                .IsConcurrencyToken();

       var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
       
       var converter = new ValueConverter<List<Genres>, string>(
           v => JsonSerializer.Serialize(v, jsonOptions),
           v => JsonSerializer.Deserialize<List<Genres>>(v, jsonOptions) ?? new List<Genres>());

       var comparer = new ValueComparer<List<Genres>>(
           (c1, c2) => c1 == null && c2 == null || c1 != null && c2 != null && c1.SequenceEqual(c2),
           c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
           c => c == null ? null : c.ToList());

       builder.Property(b => b.genres)
           .HasConversion(converter)
           .Metadata.SetValueComparer(comparer);
        

    }
}
