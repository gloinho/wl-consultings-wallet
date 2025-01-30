namespace WlConsultings.BankChallenge.Application.Dtos.Request
{
    /// <summary>
    /// Represents a request to transfer an amount to a receiver.
    /// </summary>
    public class TransferRequest
    {
        /// <summary>
        /// Gets or sets the amount to be transferred.
        /// </summary>
        public required decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the email of the receiver user.
        /// </summary>
        public required string ReceiverUserEmail { get; set; }
    }
}
