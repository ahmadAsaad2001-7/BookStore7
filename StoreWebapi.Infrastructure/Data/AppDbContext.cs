using StoreWebapi.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Infrastructure.Data;

public class AppDbContext: IdentityDbContext<user,IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<comment> Comments { get; set; }
    public DbSet<transaction> Transactions { get; set; }
    public DbSet<coupons> Coupons { get; set; }
    public DbSet<couponUser> CouponUsers { get; set; }
    public DbSet<gridCell> GridCells { get; set; }
    public DbSet<vote> Votes { get; set; }
    public DbSet<voteParticipant> VoteParticipants { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<UserBook>  UserBooks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new userConfiguration());
        modelBuilder.ApplyConfiguration(new VoteConfiguration());
        modelBuilder.ApplyConfiguration(new VoteParticipantConfiguration());
        modelBuilder.ApplyConfiguration(new CouponsConfiguration());
        modelBuilder.ApplyConfiguration(new CouponUserConfiguration());
        modelBuilder.ApplyConfiguration(new GridCellConfiguration());
        modelBuilder.ApplyConfiguration(new UserBookConfig());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        
        // Seed Data
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                id = Guid.Parse("b1111111-1111-1111-1111-111111111111"),
                name = "The Clean Coder",
                description = "A Code of Conduct for Professional Programmers.",
                author = "Robert C. Martin",
                price = 35.99m,
                isbn = "978-0137081073",
                imageUrl = "https://example.com/cleancoder.jpg",
                rating = 4.8,
                Version = 1
            },
            new Book
            {
                id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                name = "The Fellowship of the Ring",
                description = "The first volume of J.R.R. Tolkien's epic adventure.",
                author = "J.R.R. Tolkien",
                price = 19.99m,
                isbn = "978-0547928210",
                imageUrl = "https://example.com/lotr.jpg",
                rating = 4.9,
                Version = 1
            }
        ); 
        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid> 
            { 
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                Name = "USER", 
                NormalizedName = "USER",
                ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
            },
            new IdentityRole<Guid> 
            { 
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), 
                Name = "ADMINISTRATOR", 
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
            },
            new IdentityRole<Guid> 
            { 
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                Name = "VENDOR", 
                NormalizedName = "VENDOR",
                ConcurrencyStamp = "33333333-3333-3333-3333-333333333333"
            }
        );  
    }
}
