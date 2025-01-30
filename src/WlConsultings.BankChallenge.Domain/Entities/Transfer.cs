namespace WlConsultings.BankChallenge.Domain.Entities
{
    public class Transfer : BaseEntity
    {
        public decimal Amount { get; set; }
        public Guid SenderWalletId { get; set; }
        public virtual Wallet? SenderWallet { get; set; }
        public Guid ReceiverWalletId { get; set; }
        public virtual Wallet? ReceiverWallet { get; set; }
    }
}
