using System.Net;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;

namespace WlConsultings.BankChallenge.Application.Services
{
    /// <summary>
    /// Service for managing wallet operations such as deposit and balance check.
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletService"/> class.
        /// </summary>
        /// <param name="walletRepository">The wallet repository.</param>
        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        /// <summary>
        /// Deposits a specified amount into the user's wallet.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <param name="userEmail">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the wallet response.</returns>
        /// <exception cref="HttpRequestException">Thrown when the wallet is not found.</exception>
        public async Task<WalletResponse> Deposit(decimal amount, Guid userEmail)
        {
            Wallet? wallet = await _walletRepository.GetWalletByUserId(userEmail) ??
                throw new HttpRequestException(message: "Wallet not found", null, HttpStatusCode.NotFound);

            wallet.Balance += amount;

            await _walletRepository.UpdateWallet(wallet);

            return new WalletResponse
            {
                Id = wallet.Id,
                Balance = wallet.Balance
            };
        }

        /// <summary>
        /// Checks the balance of the user's wallet.
        /// </summary>
        /// <param name="userEmail">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the wallet response.</returns>
        /// <exception cref="HttpRequestException">Thrown when the wallet is not found.</exception>
        public async Task<WalletResponse> CheckBalance(Guid userEmail)
        {
            var wallet = await _walletRepository.GetWalletByUserId(userEmail) ??
                throw new HttpRequestException(message: "Wallet not found", null, HttpStatusCode.NotFound);

            return new WalletResponse
            {
                Id = wallet.Id,
                Balance = wallet.Balance
            };
        }
    }
}
