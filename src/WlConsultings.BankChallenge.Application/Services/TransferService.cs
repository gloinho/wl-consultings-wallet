using System.Net;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;

namespace WlConsultings.BankChallenge.Application.Services
{
    /// <summary>
    /// Service for handling transfer operations.
    /// </summary>
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IValidatorService _validatorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferService"/> class.
        /// </summary>
        /// <param name="transferRepository">The transfer repository.</param>
        /// <param name="walletRepository">The wallet repository.</param>
        public TransferService(ITransferRepository transferRepository, IValidatorService validatorService, IWalletRepository walletRepository)
        {
            _transferRepository = transferRepository;
            _walletRepository = walletRepository;
            _validatorService = validatorService;
        }

        /// <summary>
        /// Initiates a transfer from a user to another user.
        /// </summary>
        /// <param name="request">The transfer request containing the amount and receiver's email.</param>
        /// <param name="userId">The ID of the user initiating the transfer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the transfer response.</returns>
        /// <exception cref="HttpRequestException">Thrown when the origin or destination wallet is not found or if there are insufficient funds.</exception>
        public async Task<TransferResponse> Transfer(TransferRequest request, Guid userId)
        {
            _validatorService.ValidateAndThrow(request);
            Wallet? originWallet = await _walletRepository.GetWalletByUserId(userId)
                ?? throw new HttpRequestException(message: "Wallet not found", null, HttpStatusCode.NotFound);

            Wallet destinationWallet = await _walletRepository.GetWalletByUserEmail(request.ReceiverUserEmail)
                ?? throw new HttpRequestException(message: "Destination wallet not found", null, HttpStatusCode.NotFound);

            if (originWallet.Balance < request.Amount)
            {
                throw new HttpRequestException(message: "Insufficient funds", null, HttpStatusCode.BadRequest);
            }

            originWallet.Balance -= request.Amount;
            destinationWallet.Balance += request.Amount;

            Transfer transfer = new()
            {
                Amount = request.Amount,
                SenderWalletId = originWallet.Id,
                ReceiverWalletId = destinationWallet.Id
            };

            await _transferRepository.AddTransfer(transfer);
            await _walletRepository.UpdateWallet(originWallet);
            await _walletRepository.UpdateWallet(destinationWallet);

            return new TransferResponse
            {
                Id = transfer.Id,
                Amount = transfer.Amount,
                SenderWalletId = transfer.SenderWalletId,
                ReceiverWalletId = transfer.ReceiverWalletId,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Retrieves all transfers sent by a specific user within a date range.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="from">The start date of the range (optional).</param>
        /// <param name="to">The end date of the range (optional).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of transfer responses.</returns>
        public async Task<IEnumerable<TransferResponse>> GetAllTransfersSentByUserId(Guid userId, DateTime? from, DateTime? to)
        {
            from = from?.ToUniversalTime();
            to = to?.ToUniversalTime();

            var transfers = await _transferRepository.GetAllTransfersSentByUserId(userId, from, to);

            if (transfers == null || !transfers.Any())
            {
                return [];
            }

            return transfers.Select(t => new TransferResponse
            {
                Id = t.Id,
                Amount = t.Amount,
                SenderWalletId = t.SenderWalletId,
                ReceiverWalletId = t.ReceiverWalletId,
                CreatedAt = t.CreatedAt
            });
        }
    }
}
