using Microsoft.EntityFrameworkCore;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;
using WlConsultings.BankChallenge.Infra.Configuration;

namespace WlConsultings.BankChallenge.Infra.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly ApplicationDbContext _context;
        public TransferRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddTransfer(Transfer transfer)
        {
            await _context.Transfers.AddAsync(transfer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transfer>?> GetAllTransfersSentByUserId(Guid userId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Transfers.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= toDate.Value);
            }

            IEnumerable<Transfer>? transfer = await query
                .Where(t => t.SenderWallet!.User!.Id == userId)
                .ToListAsync();

            return transfer;
        }
    }
}
