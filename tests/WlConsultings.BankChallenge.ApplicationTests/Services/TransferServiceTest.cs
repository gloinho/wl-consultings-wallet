using AutoFixture;
using Moq;
using Shouldly;
using System.Net;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Application.Services;
using WlConsultings.BankChallenge.Domain.Entities;
using WlConsultings.BankChallenge.Domain.Interfaces.Repository;
using WlConsultings.BankChallenge.Tests.Configuration;

namespace WlConsultings.BankChallenge.Tests.Services
{
    public class TransferServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<ITransferRepository> _transferRepository;
        private readonly Mock<IWalletRepository> _walletRepository;
        private readonly Mock<IValidatorService> _validatorService;
        private readonly TransferService _transferService;

        public TransferServiceTest()
        {
            _fixture = FixtureConfig.Get();
            _transferRepository = new Mock<ITransferRepository>();
            _walletRepository = new Mock<IWalletRepository>();
            _validatorService = new Mock<IValidatorService>();

            _transferService = new TransferService(_transferRepository.Object, _validatorService.Object, _walletRepository.Object);
        }

        [Fact]
        public async Task Transfer_ValidRequest_ReturnsTransferResponse()
        {
            // Arrange
            var request = _fixture.Build<TransferRequest>()
                                  .With(x => x.ReceiverUserEmail, "receiver@example.com")
                                  .With(x => x.Amount, 100) // Define um valor válido para a transferência
                                  .Create();

            var userId = Guid.NewGuid();

            var originWallet = _fixture.Build<Wallet>()
                                       .With(x => x.UserId, userId)
                                       .With(x => x.Balance, 200) // Saldo suficiente para a transferência
                                       .Create();

            var destinationWallet = _fixture.Build<Wallet>()
                                            .With(x => x.Balance, 50) // Saldo inicial do destinatário
                                            .Create();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync(originWallet);

            _walletRepository.Setup(r => r.GetWalletByUserEmail(request.ReceiverUserEmail))
                             .ReturnsAsync(destinationWallet);

            _transferRepository.Setup(r => r.AddTransfer(It.IsAny<Transfer>()))
                               .Returns(Task.CompletedTask);

            _walletRepository.Setup(r => r.UpdateWallet(It.IsAny<Wallet>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _transferService.Transfer(request, userId);

            // Assert
            result.ShouldNotBeNull();
            result.Amount.ShouldBe(request.Amount);
            result.SenderWalletId.ShouldBe(originWallet.Id);
            result.ReceiverWalletId.ShouldBe(destinationWallet.Id);

            _walletRepository.Verify(r => r.UpdateWallet(It.IsAny<Wallet>()), Times.Exactly(2));
            _transferRepository.Verify(r => r.AddTransfer(It.IsAny<Transfer>()), Times.Once);
            originWallet.Balance.ShouldBe(100); // Saldo da carteira de origem após a transferência
            destinationWallet.Balance.ShouldBe(150); // Saldo da carteira de destino após a transferência   
        }

        [Fact]
        public async Task Transfer_OriginWalletNotFound_ThrowsHttpRequestException()
        {
            // Arrange
            var request = _fixture.Build<TransferRequest>()
                                  .With(x => x.Amount, 100)
                                  .With(x => x.ReceiverUserEmail, "receiver@example.com")
                                  .Create();

            var userId = Guid.NewGuid();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync((Wallet)null); // Simula que a carteira de origem não foi encontrada

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _transferService.Transfer(request, userId));
            exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            exception.Message.ShouldBe("Wallet not found");
        }

        [Fact]
        public async Task Transfer_DestinationWalletNotFound_ThrowsHttpRequestException()
        {
            // Arrange
            var request = _fixture.Build<TransferRequest>()
                                  .With(x => x.Amount, 100)
                                  .With(x => x.ReceiverUserEmail, "receiver@example.com")
                                  .Create();

            var userId = Guid.NewGuid();

            var originWallet = _fixture.Build<Wallet>()
                                       .With(x => x.UserId, userId)
                                       .With(x => x.Balance, 200)
                                       .Create();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync(originWallet);

            _walletRepository.Setup(r => r.GetWalletByUserEmail(request.ReceiverUserEmail))
                             .ReturnsAsync((Wallet)null); // Simula que a carteira de destino não foi encontrada

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _transferService.Transfer(request, userId));
            exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            exception.Message.ShouldBe("Destination wallet not found");
        }

        [Fact]
        public async Task Transfer_InsufficientFunds_ThrowsHttpRequestException()
        {
            // Arrange
            var request = _fixture.Build<TransferRequest>()
                                  .With(x => x.Amount, 300) // Valor maior que o saldo da carteira de origem
                                  .With(x => x.ReceiverUserEmail, "receiver@example.com")
                                  .Create();

            var userId = Guid.NewGuid();

            var originWallet = _fixture.Build<Wallet>()
                                       .With(x => x.UserId, userId)
                                       .With(x => x.Balance, 200) // Saldo insuficiente para a transferência
                                       .Create();

            var destinationWallet = _fixture.Build<Wallet>()
                                            .With(x => x.Balance, 50)
                                            .Create();

            _walletRepository.Setup(r => r.GetWalletByUserId(userId))
                             .ReturnsAsync(originWallet);

            _walletRepository.Setup(r => r.GetWalletByUserEmail(request.ReceiverUserEmail))
                             .ReturnsAsync(destinationWallet);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => _transferService.Transfer(request, userId));
            exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            exception.Message.ShouldBe("Insufficient funds");
        }

        [Fact]
        public async Task GetAllTransfersSentByUserId_ReturnsListOfTransferResponses()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var from = DateTime.UtcNow.AddDays(-7);
            var to = DateTime.UtcNow;

            var transfers = _fixture.Build<Transfer>()
                                    .With(x => x.SenderWalletId, userId)
                                    .CreateMany(3)
                                    .ToList();

            _transferRepository.Setup(r => r.GetAllTransfersSentByUserId(userId, from, to))
                               .ReturnsAsync(transfers);

            // Act
            var result = await _transferService.GetAllTransfersSentByUserId(userId, from, to);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(transfers.Count);
            _transferRepository.Verify(r => r.GetAllTransfersSentByUserId(userId, from, to), Times.Once);
        }

        [Fact]
        public async Task GetAllTransfersSentByUserId_NoTransfers_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var from = DateTime.UtcNow.AddDays(-7);
            var to = DateTime.UtcNow;

            _transferRepository.Setup(r => r.GetAllTransfersSentByUserId(userId, from, to))
                               .ReturnsAsync((List<Transfer>)null); // Simula que não há transferências

            // Act
            var result = await _transferService.GetAllTransfersSentByUserId(userId, from, to);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            _transferRepository.Verify(r => r.GetAllTransfersSentByUserId(userId, from, to), Times.Once);
        }
    }
}