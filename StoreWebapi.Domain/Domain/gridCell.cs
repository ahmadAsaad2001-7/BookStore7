namespace StoreWebapi.Domain.Domain;
public class gridCell
{

    public string cellId { get; set; } = null!;
    public double latitude { get; set; }
    public double longitude { get; set; }
    public int precision { get; set; }
    public ICollection<user> Users { get; set; } = new List<user>();
}