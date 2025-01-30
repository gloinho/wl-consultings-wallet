namespace WlConsultings.BankChallenge.Application.Dto.Response
{
    /// <summary>
    /// Represents a response containing wallet information.
    /// </summary>
    public class WalletResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the wallet.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the balance of the wallet.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
