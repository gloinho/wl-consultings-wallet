using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;

namespace WlConsultings.BankChallenge.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing wallet operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletController"/> class.
        /// </summary>
        /// <param name="service">The wallet service.</param>
        public WalletController(IWalletService service)
        {
            _service = service;
        }

        /// <summary>
        /// Deposits the specified amount into the authenticated user's wallet.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <returns>The response containing the updated wallet information.</returns>
        /// <response code="200">Returns the updated wallet information.</response>
        /// <response code="404">If the wallet is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPost("deposit/me")]
        [ProducesResponseType(typeof(ApiResponse<WalletResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<WalletResponse>>> Deposit(decimal amount)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ret = await _service.Deposit(amount, userId);
            return Ok(ret);
        }

        /// <summary>
        /// Checks the balance of the authenticated user's wallet.
        /// </summary>
        /// <returns>The response containing the wallet balance information.</returns>
        /// <response code="200">Returns the wallet balance information.</response>
        /// <response code="404">If the wallet is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("balance/me")]
        [ProducesResponseType(typeof(ApiResponse<WalletResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<WalletResponse>>> CheckBalance()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ret = await _service.CheckBalance(userId);
            return Ok(ret);
        }

        /// <summary>
        /// Deposits the specified amount into the specified user's wallet. Admin only.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="amount">The amount to deposit.</param>
        /// <returns>The response containing the updated wallet information.</returns>
        /// <response code="200">Returns the updated wallet information.</response>
        /// <response code="404">If the wallet is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [Authorize(Roles = nameof(RoleTypeDto.ADMIN))]
        [HttpPost("deposit/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<WalletResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<WalletResponse>>> Deposit([FromRoute] Guid userId, decimal amount)
        {
            var ret = await _service.Deposit(amount, userId);
            return Ok(ret);
        }

        /// <summary>
        /// Checks the balance of the specified user's wallet. Admin only.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The response containing the wallet balance information.</returns>
        /// <response code="200">Returns the wallet balance information.</response>
        /// <response code="404">If the wallet is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("balance/{userId}")]
        [Authorize(Roles = nameof(RoleTypeDto.ADMIN))]
        [ProducesResponseType(typeof(ApiResponse<WalletResponse>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<WalletResponse>>> CheckBalance([FromRoute] Guid userId)
        {
            var ret = await _service.CheckBalance(userId);
            return Ok(ret);
        }
    }
}
