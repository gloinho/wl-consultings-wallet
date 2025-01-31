using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WlConsultings.BankChallenge.Application.Dto.Request;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Application.Utils;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Enum;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;

namespace WlConsultings.BankChallenge.Application.Services
{
    /// <summary>
    /// Service class for handling user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IValidatorService _validatorService;
        private readonly IUserRepository _userRepository;
        private readonly string _jwtKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="configuration">The configuration.</param>
        public UserService(IUserRepository userRepository, IValidatorService validatorService, IConfiguration configuration)
        {
            _jwtKey = configuration["JWT:KEY"] ?? "096c0a72c31f9a2d65126d8e8a401a2ab2f2e21d0a282a6ffe6642bbef65ffd9";
            _userRepository = userRepository;
            _validatorService = validatorService;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The request containing user details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user information.</returns>
        /// <exception cref="HttpRequestException">Thrown when the user already exists.</exception>
        public async Task<UserResponse> CreateUser(CreateUserRequest request)
        {
            _validatorService.ValidateAndThrow(request);
            var existingUser = await _userRepository.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                throw new HttpRequestException("User already exists", null, HttpStatusCode.BadRequest);
            }
            var user = new User
            {
                Email = request.Email,
                Password = PasswordHasher.HashPassword(request.Password),
                Role = (RoleType)Enum.Parse(typeof(RoleType), request.Role.ToString()),
                Wallet = new Wallet
                {
                    Balance = 0
                }
            };
            await _userRepository.CreateUser(user);
            return new() { Email = user.Email, Id = user.Id, Role = request.Role };
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of user information.</returns>
        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(x => new UserResponse
            {
                Email = x.Email,
                Id = x.Id,
                Role = (RoleTypeDto)Enum.Parse(typeof(RoleTypeDto), x.Role.ToString()),
            });
        }

        /// <summary>
        /// Authenticates a user and returns a login response.
        /// </summary>
        /// <param name="request">The request containing login details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the login response.</returns>
        /// <exception cref="HttpRequestException">Thrown when the user does not exist or the password is invalid.</exception>
        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            _validatorService.ValidateAndThrow(request);
            var user = await _userRepository.GetUserByEmail(request.Email) ??
                throw new HttpRequestException("User not found.", null, HttpStatusCode.NotFound);

            if (!PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                throw new HttpRequestException("Invalid Password", null, HttpStatusCode.BadRequest);
            }

            var token = GenerateJwtToken(user);
            return new()
            {
                Token = token
            };
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>The generated JWT token.</returns>
        private string GenerateJwtToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("Group","grupo.teste")
                };

            var token = new JwtSecurityToken(
                issuer: "wlconsultings-bank.com",
                audience: "wlconsultings-bank.com",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
