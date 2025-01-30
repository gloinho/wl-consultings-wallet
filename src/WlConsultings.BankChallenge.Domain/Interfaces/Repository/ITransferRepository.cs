using WlConsultings.BankChallenge.Domain.Entities;

namespace WlConsultings.BankChallenge.Domain.Interfaces.Repository
{
    public interface ITransferRepository
    {
        Task AddTransfer(Transfer transfer);
        Task<IEnumerable<Transfer>?> GetAllTransfersSentByUserId(Guid userId, DateTime? fromDate, DateTime? toDate);
    }
}
