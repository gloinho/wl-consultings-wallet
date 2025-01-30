using Microsoft.EntityFrameworkCore;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;
using WlConsultings.BankChallenge.Infra.Configuration;

namespace WlConsultings.BankChallenge.Infra.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetWalletByUserId(Guid userId)
        {
            return await _context.Wallets.Where(t => t.User!.Id == userId).FirstOrDefaultAsync();
        }
        public async Task<Wallet?> GetWalletByUserEmail(string email)
        {
            return await _context.Wallets.Where(t => t.User!.Email == email).FirstOrDefaultAsync();
        }

        public async Task UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
