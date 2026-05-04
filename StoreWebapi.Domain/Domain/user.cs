using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Domain.Domain;
public class user : IdentityUser<Guid>
{
    Roles _role=Roles.USER;
    public string imageUrl { get; set; }
    public bool isSusbended { get; set; } = false;
    public string? cellId { get; set; }
    public gridCell gridCell { get; set; }
    public ICollection<transaction> Transactions { get; set; }
    public ICollection<Book> BooksRelated { get; set; }
    public ICollection<Order> Orders { get; set; }
    public DateTime? SuspendedAt { get; set; }
    
    public DateTime? SuspendsUntil { get; set; }
    public string? SuspensionReason { get; set; }

    public ICollection<transaction> VendorTransactions { get; set; }

    public ICollection<comment> Comments { get; set; }

    public ICollection<couponUser> CouponUsers { get; set; }

    public ICollection<vote> VotesInitiated { get; set; }
    public ICollection<UserBook> UserBooks { get; set; }

    public ICollection<voteParticipant> ParticipatingVotes { get; set; }

}