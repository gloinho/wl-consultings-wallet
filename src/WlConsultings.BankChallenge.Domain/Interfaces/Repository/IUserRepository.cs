using WlConsultings.BankChallenge.Domain.Entities;

namespace WlConsultings.BankChallenge.Domain.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
