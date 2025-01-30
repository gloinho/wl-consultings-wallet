using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;

namespace WlConsultings.BankChallenge.WebApi.Controllers
{
    /// <summary>
    /// Controller for handling transfer-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferController"/> class.
        /// </summary>
        /// <param name="transferService">The transfer service.</param>
        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        /// <summary>
        /// Gets all transfers sent by a specific user. Admin only.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="from">The start date filter</param>
        /// <param name="to">The end date filter.</param>
        /// <returns>A list of transfers.</returns>
        /// <response code="200">Returns the list of transfers.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("sent/{userId}")]
        [Authorize(Roles = nameof(RoleTypeDto.ADMIN))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TransferResponse>>> GetAllTransfersSentByUserId([FromRoute] Guid userId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var transfers = await _transferService.GetAllTransfersSentByUserId(userId, from, to);
            return Ok(transfers);
        }

        /// <summary>
        /// Gets all transfers sent by the authenticated user.
        /// </summary>
        /// <param name="from">The start date filter.</param>
        /// <param name="to">The end date filter.</param>
        /// <returns>A list of transfers.</returns>
        /// <response code="200">Returns the list of transfers.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("sent/me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TransferResponse>>> GetAllTransfersSentByUserId([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var transfers = await _transferService.GetAllTransfersSentByUserId(userId, from, to);
            return Ok(transfers);
        }

        /// <summary>
        /// Initiates a transfer.
        /// </summary>
        /// <param name="request">The transfer request.</param>
        /// <returns>The transfer response.</returns>
        /// <response code="201">Transfer created successfully.</response>
        /// <response code="400">Bad request, invalid input.</response>
        /// <response code="404">User or wallet not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransferResponse>> Transfer([FromBody] TransferRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var transfer = await _transferService.Transfer(request, userId);
            return CreatedAtAction(nameof(Transfer), transfer);
        }
    }
}
