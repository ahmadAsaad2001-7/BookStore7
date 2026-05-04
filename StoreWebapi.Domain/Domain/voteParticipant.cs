namespace StoreWebapi.Domain.Domain;
public class voteParticipant
{
    public Guid voteId { get; set; }
    public vote? vote { get; set; }
    public Guid userId { get; set; }
    public user? user { get; set; }
    public bool Approval { get; set; } 
    public DateTime VotedAt { get; set; }
}