using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System.Security.Claims;
using WlConsultings.BankChallenge.Application.Dto.Request;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Tests.Configuration;
using WlConsultings.BankChallenge.WebApi.Controllers;

namespace WlConsultings.BankChallenge.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUserService> _userService;
        private readonly UserController _controller;
        private readonly Guid _userId = Guid.NewGuid();

        public UserControllerTest()
        {
            _fixture = FixtureConfig.Get();
            _userService = new Mock<IUserService>();
            _controller = new UserController(_userService.Object);

            // Configura o contexto do usuário autenticado
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString()),
                new Claim(ClaimTypes.Role, nameof(RoleTypeDto.ADMIN))
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task CreateUser_ValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var request = _fixture.Create<CreateUserRequest>();
            var userResponse = _fixture.Create<UserResponse>();

            _userService.Setup(s => s.CreateUser(request))
                        .ReturnsAsync(userResponse);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            var createdResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
            createdResult.ActionName.ShouldBe(nameof(UserController.CreateUser));
            createdResult.Value.ShouldBeOfType<UserResponse>();
            createdResult.Value.ShouldBe(userResponse);
        }

        [Fact]
        public async Task LoginUser_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = _fixture.Create<LoginRequest>();
            var loginResponse = _fixture.Create<LoginResponse>();

            _userService.Setup(s => s.LoginUser(request))
                        .ReturnsAsync(loginResponse);
            // Act
            var result = await _controller.LoginUser(request);

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            okResult.Value.ShouldBeOfType<LoginResponse>();
            okResult.Value.ShouldBe(loginResponse);
        }

        [Fact]
        public async Task GetAllUsers_AdminRole_ReturnsOkResult()
        {
            // Arrange
            var users = _fixture.CreateMany<UserResponse>(3).ToList();

            _userService.Setup(s => s.GetAllUsers())
                        .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            okResult.Value.ShouldBeOfType<List<UserResponse>>();
            okResult.Value.ShouldBe(users);
        }
    }
}