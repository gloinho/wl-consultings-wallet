using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WlConsultings.BankChallenge.Application.Dto.Request;
using WlConsultings.BankChallenge.Application.Dto.Response;
using WlConsultings.BankChallenge.Application.Dtos.Request;
using WlConsultings.BankChallenge.Application.Dtos.Response;
using WlConsultings.BankChallenge.Application.Interfaces;

namespace WlConsultings.BankChallenge.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The request containing user details.</param>
        /// <returns>The created user response.</returns>
        /// <response code="201">Returns the created user response</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] CreateUserRequest request)
        {

            var ret = await _userService.CreateUser(request);
            return CreatedAtAction(nameof(CreateUser), ret);

        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="request">The request containing login details.</param>
        /// <returns>The login response.</returns>
        /// <response code="200">Returns the login response</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If the user is not found</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> LoginUser([FromBody] LoginRequest request)
        {
            var ret = await _userService.LoginUser(request);
            return Ok(ret);
        }

        /// <summary>
        /// Gets all users. Only accessible by admin users.
        /// </summary>
        /// <returns>The list of all users.</returns>
        /// <response code="200">Returns the list of all users</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [Authorize(Roles = nameof(RoleTypeDto.ADMIN))]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> GetAllUsers()
        {
            var ret = await _userService.GetAllUsers();
            return Ok(ret);
        }
    }
}
