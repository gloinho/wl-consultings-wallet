using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;

namespace WlConsultings.BankChallenge.Application.Interfaces
{
    /// <summary>
    /// Interface for transfer services.
    /// </summary>
    public interface ITransferService
    {
        /// <summary>
        /// Retrieves all transfers sent by a specific user within a date range.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="from">The start date of the range (optional).</param>
        /// <param name="to">The end date of the range (optional).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of transfer responses.</returns>
        Task<IEnumerable<TransferResponse>> GetAllTransfersSentByUserId(Guid userId, DateTime? from, DateTime? to);

        /// <summary>
        /// Initiates a transfer from a user to another user.
        /// </summary>
        /// <param name="request">The transfer request containing the amount and receiver's email.</param>
        /// <param name="userId">The ID of the user initiating the transfer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the transfer response.</returns>
        Task<TransferResponse> Transfer(TransferRequest request, Guid userId);
    }
}
