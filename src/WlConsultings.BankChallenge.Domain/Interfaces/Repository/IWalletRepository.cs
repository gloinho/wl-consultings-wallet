using WlConsultings.BankChallenge.Domain.Entities;

namespace WlConsultings.BankChallenge.Domain.Interfaces.Repository
{
    public interface IWalletRepository
    {
        Task UpdateWallet(Wallet wallet);
        Task<Wallet?> GetWalletByUserId(Guid userId);
        Task<Wallet?> GetWalletByUserEmail(string email);
    }
}
