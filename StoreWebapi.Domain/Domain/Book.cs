using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Domain.Domain;
public class Book
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string author  { get; set; }
    public decimal price { get; set; }
    public string isbn { get; set; }
    public string imageUrl  { get; set; }
    public Guid? UserId { get; set; }
    public user user { get; set; }
    public List<Genres>? genres { get; set; }
    public int Version { get; set; }
    public double rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<comment>  Comments { get; set; }
    public ICollection<transaction>  Transactions { get; set; }
    public IEnumerable<UserBook>? UserBooks { get; set; }
}