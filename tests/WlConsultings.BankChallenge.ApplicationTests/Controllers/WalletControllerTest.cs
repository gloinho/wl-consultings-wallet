using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System.Security.Claims;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Tests.Configuration;
using WlConsultings.BankChallenge.WebApi.Controllers;

namespace WlConsultings.BankChallenge.Tests.Controllers
{
    public class WalletControllerTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IWalletService> _walletService;
        private readonly WalletController _controller;
        private readonly Guid _userId = Guid.NewGuid();

        public WalletControllerTest()
        {
            _fixture = FixtureConfig.Get();
            _walletService = new Mock<IWalletService>();
            _controller = new WalletController(_walletService.Object);

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
        public async Task Deposit_AuthenticatedUser_ReturnsOkResult()
        {
            // Arrange
            var amount = _fixture.Create<decimal>();
            var walletResponse = _fixture.Create<WalletResponse>();

            _walletService.Setup(s => s.Deposit(amount, _userId))
                          .ReturnsAsync(walletResponse);

            // Act
            var result = await _controller.Deposit(amount);

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            okResult.Value.ShouldBeOfType<WalletResponse>();
            okResult.Value.ShouldBe(walletResponse);
        }

        [Fact]
        public async Task CheckBalance_AuthenticatedUser_ReturnsOkResult()
        {
            // Arrange
            var walletResponse = _fixture.Create<WalletResponse>();

            _walletService.Setup(s => s.CheckBalance(_userId))
                          .ReturnsAsync(walletResponse);

            // Act
            var result = await _controller.CheckBalance();

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            okResult.Value.ShouldBeOfType<WalletResponse>();
            okResult.Value.ShouldBe(walletResponse);
        }

        [Fact]
        public async Task Deposit_AdminRole_ReturnsOkResult()
        {
            // Arrange
            var userId = _fixture.Create<Guid>();
            var amount = _fixture.Create<decimal>();
            var walletResponse = _fixture.Create<WalletResponse>();

            _walletService.Setup(s => s.Deposit(amount, userId))
                          .ReturnsAsync(walletResponse);

            // Act
            var result = await _controller.Deposit(userId, amount);

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            okResult.Value.ShouldBeOfType<WalletResponse>();
            okResult.Value.ShouldBe(walletResponse);
        }

        [Fact]
        public async Task CheckBalance_AdminRole_ReturnsOkResult()
        {
            // Arrange
            var userId = _fixture.Create<Guid>();
            var walletResponse = _fixture.Create<WalletResponse>();

            _walletService.Setup(s => s.CheckBalance(userId))
                          .ReturnsAsync(walletResponse);

            // Act
            var result = await _controller.CheckBalance(userId);

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            okResult.Value.ShouldBeOfType<WalletResponse>();
            okResult.Value.ShouldBe(walletResponse);
        }
    }
}