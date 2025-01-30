namespace WlConsultings.BankChallenge.Application.Dtos.Response
{
    /// <summary>
    /// Represents the response for a transfer operation.
    /// </summary>
    public class TransferResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the transfer.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the amount transferred.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the sender's wallet.
        /// </summary>
        public Guid SenderWalletId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the receiver's wallet.
        /// </summary>
        public Guid ReceiverWalletId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the transfer was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
