using AutoFixture;
using Moq;
using Shouldly;
using System.Net;
using WlConsultings.BankChallenge.Application.Services;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;
using WlConsultings.BankChallenge.Tests.Configuration;

namespace WlConsultings.BankChallenge.Tests.Services
{
    public class WalletServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IWalletRepository> _walletRepository;
        private readonly WalletService _walletService;

        public WalletServiceTest()
        {
            _fixture = FixtureConfig.Get();
            _walletRepository = new Mock<IWalletRepository>();
            _walletService = new WalletService(_walletRepository.Object);
        }

        [Fact]
        public async Task Deposit_ValidRequest_ReturnsWalletResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var amount = 100;
            var wallet = _fixture.Build<Wallet>()
                                 .With(x => x.UserId, userId)
                                 .With(x => x.Balance, 200) // Saldo inicial
                                 .Create();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync(wallet);

            _walletRepository.Setup(r => r.UpdateWallet(It.IsAny<Wallet>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _walletService.Deposit(amount, userId);

            // Assert
            result.ShouldNotBeNull();
            result.Balance.ShouldBe(300);
        }

        [Fact]
        public async Task Deposit_WalletNotFound_ThrowsHttpRequestException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var amount = 100;

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync((Wallet)null); // Simula que a carteira não foi encontrada

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _walletService.Deposit(amount, userId));
            exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            exception.Message.ShouldBe("Wallet not found");
        }

        [Fact]
        public async Task CheckBalance_ValidRequest_ReturnsWalletResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var wallet = _fixture.Build<Wallet>()
                                 .With(x => x.UserId, userId)
                                 .With(x => x.Balance, 500) // Saldo inicial
                                 .Create();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync(wallet);

            // Act
            var result = await _walletService.CheckBalance(userId);

            // Assert
            result.ShouldNotBeNull();
            result.Balance.ShouldBe(wallet.Balance);
            _walletRepository.Verify(r => r.GetWalletByUserId(userId), Times.Once);
        }

        [Fact]
        public async Task CheckBalance_WalletNotFound_ThrowsHttpRequestException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync((Wallet)null); // Simula que a carteira não foi encontrada

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _walletService.CheckBalance(userId));
            exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            exception.Message.ShouldBe("Wallet not found");
        }
    }
}