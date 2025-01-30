using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using System.Net;
using WlConsultings.BankChallenge.Application.Dto.Request;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Application.Services;
using WlConsultings.BankChallenge.Application.Utils;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Enum;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;
using WlConsultings.BankChallenge.Tests.Configuration;

namespace WlConsultings.BankChallenge.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IValidatorService> _validatorService;
        private readonly Mock<IConfiguration> _configuration;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _fixture = FixtureConfig.Get();
            _userRepository = new Mock<IUserRepository>();
            _validatorService = new Mock<IValidatorService>();
            _configuration = new Mock<IConfiguration>();

            _userService = new UserService(_userRepository.Object, _validatorService.Object, _configuration.Object);
        }

        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsUserResponse()
        {
            // Arrange
            var request = _fixture.Build<CreateUserRequest>().Create();

            var user = _fixture.Build<User>()
                               .With(x => x.Email, request.Email)
                               .With(x => x.Password, PasswordHasher.HashPassword(request.Password))
                               .With(x => x.Role, (RoleType)Enum.Parse(typeof(RoleType), request.Role.ToString()))
                               .With(x => x.Wallet, new Wallet { Balance = 0 })
                               .Create();

            _userRepository.Setup(r => r.GetUserByEmail(request.Email))
                           .ReturnsAsync((User)null);
            _userRepository.Setup(r => r.CreateUser(It.IsAny<User>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateUser(request);

            // Assert
            result.ShouldNotBeNull();
            result.Email.ShouldBe(request.Email);
            result.Role.ShouldBe(request.Role);
            _userRepository.Verify(r => r.CreateUser(It.Is<User>(u =>
                u.Email == request.Email &&
                u.Role == (RoleType)Enum.Parse(typeof(RoleType), request.Role.ToString()) &&
                u.Wallet!.Balance == 0
            )), Times.Once);
        }

        [Fact]
        public async Task CreateUser_UserAlreadyExists_ThrowsHttpRequestException()
        {
            // Arrange
            var request = _fixture.Build<CreateUserRequest>().Create();

            var existingUser = _fixture.Build<User>()
                                       .With(x => x.Email, request.Email)
                                       .Create();

            _userRepository.Setup(r => r.GetUserByEmail(request.Email))
                           .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _userService.CreateUser(request));
            exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            exception.Message.ShouldBe("User already exists");
        }

        [Fact]
        public async Task GetAllUsers_ReturnsListOfUserResponses()
        {
            // Arrange
            var users = _fixture.Build<User>()
                                .With(x => x.Wallet, new Wallet { Balance = 0 })
                                .CreateMany(3)
                                .ToList();

            _userRepository.Setup(r => r.GetAllUsers())
                           .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(users.Count);
            _userRepository.Verify(r => r.GetAllUsers(), Times.Once);
        }

        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var request = _fixture.Build<LoginRequest>().Create();
            var user = _fixture.Build<User>()
                               .With(x => x.Email, request.Email)
                               .With(x => x.Password, PasswordHasher.HashPassword(request.Password))
                               .With(x => x.Role, RoleType.CUSTOMER)
                               .With(t => t.Wallet, new Wallet { Balance = 0 })
                               .Create();

            _userRepository.Setup(r => r.GetUserByEmail(request.Email))
                           .ReturnsAsync(user);

            // Act
            var result = await _userService.LoginUser(request);

            // Assert
            result.ShouldNotBeNull();
            result.Token.ShouldNotBeNullOrEmpty();
            _userRepository.Verify(r => r.GetUserByEmail(request.Email), Times.Once);
        }

        [Fact]
        public async Task LoginUser_UserNotFound_ThrowsHttpRequestException()
        {
            // Arrange
            var request = _fixture.Build<LoginRequest>().Create();

            _userRepository.Setup(r => r.GetUserByEmail(request.Email))
                           .ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _userService.LoginUser(request));
            exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            exception.Message.ShouldBe("User not found.");
        }

        [Fact]
        public async Task LoginUser_InvalidPassword_ThrowsHttpRequestException()
        {
            // Arrange
            var request = _fixture.Build<LoginRequest>().Create();

            var user = _fixture.Build<User>()
                               .With(x => x.Email, request.Email)
                               .With(x => x.Password, PasswordHasher.HashPassword("WrongPassword!")) // Senha diferente da request
                               .With(x => x.Role, RoleType.CUSTOMER)
                               .With(t => t.Wallet, new Wallet { Balance = 0 })
                               .Create();

            _userRepository.Setup(r => r.GetUserByEmail(request.Email))
                           .ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _userService.LoginUser(request));
            exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            exception.Message.ShouldBe("Invalid Password");
        }
    }
}