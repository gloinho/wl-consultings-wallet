using WlConsultings.BankChallenge.Application.Dto.Response;

namespace WlConsultings.BankChallenge.Application.Interfaces
{
    /// <summary>
    /// Provides methods for wallet operations such as deposit and balance check.
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// Deposits a specified amount into the user's wallet.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the wallet response.</returns>
        Task<WalletResponse> Deposit(decimal amount, Guid userId);

        /// <summary>
        /// Checks the balance of the user's wallet.
        /// </summary>
        /// <param name="userEmail">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the wallet response.</returns>
        Task<WalletResponse> CheckBalance(Guid userEmail);
    }
}
