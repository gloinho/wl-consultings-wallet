using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System.Security.Claims;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;
using WlConsultings.BankChallenge.Tests.Configuration;
using WlConsultings.BankChallenge.WebApi.Controllers;

namespace WlConsultings.BankChallenge.Tests.Controllers
{
    public class TransferControllerTest
    {
        private readonly Fixture _fixture;
        private Mock<ITransferService> _service;
        private readonly TransferController _controller;
        private readonly Guid _userId = Guid.NewGuid();
        public TransferControllerTest()
        {
            _fixture = FixtureConfig.Get();
            _service = new();
            _controller = new(_service.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, _userId.ToString())]));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
        [Fact]
        public async Task GetAllTransfersSentByUserId_AdminRole_ReturnsOkResult()
        {
            // Arrange
            var from = _fixture.Create<DateTime>();
            var to = _fixture.Create<DateTime>();
            var transfers = _fixture.CreateMany<TransferResponse>(2).ToList();

            _service.Setup(s => s.GetAllTransfersSentByUserId(_userId, from, to))
                    .ReturnsAsync(transfers);

            // Act
            var result = await _controller.GetAllTransfersSentByUserId(_userId, from, to);

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            var returnedTransfers = okResult.Value.ShouldBeOfType<List<TransferResponse>>();
            returnedTransfers.Count().ShouldBe(transfers.Count);
        }

        [Fact]
        public async Task GetAllTransfersSentByUserId_AuthenticatedUser_ReturnsOkResult()
        {
            // Arrange
            var from = _fixture.Create<DateTime>();
            var to = _fixture.Create<DateTime>();
            var transfers = _fixture.CreateMany<TransferResponse>(2).ToList();

            _service.Setup(s => s.GetAllTransfersSentByUserId(_userId, from, to))
                    .ReturnsAsync(transfers);

            // Act
            var result = await _controller.GetAllTransfersSentByUserId(from, to);

            // Assert
            var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
            var returnedTransfers = okResult.Value.ShouldBeOfType<List<TransferResponse>>();
            returnedTransfers.Count.ShouldBe(transfers.Count);
        }

        [Fact]
        public async Task Transfer_ValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var request = _fixture.Create<TransferRequest>();
            var transferResponse = _fixture.Create<TransferResponse>();

            _service.Setup(s => s.Transfer(request, _userId))
                    .ReturnsAsync(transferResponse);
            // Act
            var result = await _controller.Transfer(request);

            // Assert
            var createdResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
            createdResult.ActionName.ShouldBe(nameof(TransferController.Transfer));
            createdResult.Value.ShouldBeOfType<TransferResponse>();
        }
    }
}
