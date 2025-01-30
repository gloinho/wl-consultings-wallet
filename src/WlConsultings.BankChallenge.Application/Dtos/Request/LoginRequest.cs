namespace WlConsultings.BankChallenge.Application.Dtos.Request
{
    /// <summary>
    /// Represents a request for user login.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }
    }
}
