namespace WlConsultings.BankChallenge.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public decimal Balance { get; set; }
        public virtual IEnumerable<Transfer>? TransfersSent { get; set; }
        public virtual IEnumerable<Transfer>? TransfersReceived { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
