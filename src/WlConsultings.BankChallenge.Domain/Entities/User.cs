using WlConsultings.BankChallenge.Domain.Enum;

namespace WlConsultings.BankChallenge.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required RoleType Role { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
