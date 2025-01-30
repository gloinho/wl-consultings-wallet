using WlConsultings.BankChallenge.Application.Dto.Request;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;

namespace WlConsultings.BankChallenge.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The request containing user details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user information.</returns>
        Task<UserResponse> CreateUser(CreateUserRequest request);

        /// <summary>
        /// Authenticates a user and returns a login response.
        /// </summary>
        /// <param name="request">The request containing login details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the login response.</returns>
        Task<LoginResponse> LoginUser(LoginRequest request);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of user information.</returns>
        Task<IEnumerable<UserResponse>> GetAllUsers();
    }
}
