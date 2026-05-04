    namespace StoreWebapi.Domain.Domain;
    public class vote
    {
        public Guid Id { get; set; }
        public Guid InitiatorId { get; set; }
        public int Approval { get; set; }
        public int disApprove { get; set; }
        public bool IsResolved { get; set; }        
        public bool Result { get; set; } 
        public DateTime expiryDate { get; set; }
        public DateTime createDate { get; set; }
        public string subject  { get; set; }
        public ICollection<voteParticipant> Participants { get; set; } = new List<voteParticipant>();
    }