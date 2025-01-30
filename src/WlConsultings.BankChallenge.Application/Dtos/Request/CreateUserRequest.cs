using WlConsultings.BankChallenge.Application.Dtos.Request;

namespace WlConsultings.BankChallenge.Application.Dto.Request
{
    /// <summary>
    /// Represents a request to create a new user.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public required RoleTypeDto Role { get; set; }
    }
}
