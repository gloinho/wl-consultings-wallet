namespace WlConsultings.BankChallenge.Application.Dtos.Response
{
    /// <summary>
    /// Represents the response returned after a successful login attempt.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Gets or sets the token issued upon successful authentication.
        /// </summary>
        public required string Token { get; set; }
    }
}
