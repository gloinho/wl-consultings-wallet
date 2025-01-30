using WlConsultings.BankChallenge.Application.Dtos.Request;

namespace WlConsultings.BankChallenge.Application.Dto.Response
{
    /// <summary>
    /// Represents a response containing user information.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public required RoleTypeDto Role { get; set; }
    }
}
