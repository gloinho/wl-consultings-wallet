namespace WlConsultings.BankChallenge.Application.Dtos.Response
{
    /// <summary>
    /// Represents a standard API response.
    /// </summary>
    /// <typeparam name="T">The type of the data included in the response.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets the data included in the response.
        /// </summary>
        public required T Data { get; set; }

        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        public required int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message included in the response.
        /// </summary>
        public required string Message { get; set; }
    }
}
