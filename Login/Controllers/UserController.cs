using AirPlane.Dto;
using Data.Models.Enum;
using Login.Helper;
using Login.Repo;
using Login.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _token;
        public UserController(IUserService userService, ITokenService token)
        {
            _userService = userService;
            _token = token;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var register = _userService.Register(registerDto);
                return Ok(register);

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            try
            {
                var register = _userService.RegisterAdmin(registerDto);
                return Ok(register);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login (LoginDto loginDto)
        {
            try
            {
                var login = _userService.Login(loginDto);
                return Ok(login);

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("activate")]
        public IActionResult ActivateUser([FromQuery] string activationToken)
        {
            if (string.IsNullOrEmpty(activationToken))
            {
                // Xử lý mã kích hoạt không hợp lệ
                return BadRequest(new { message = "Invalid activation token" });
            }

            // Gọi hàm ActivateUser để kích hoạt người dùng
            bool activationResult = _userService.ActivateUser(activationToken);

            if (activationResult)
            {
                // Kích hoạt thành công
                return Ok(new { message = "Activation successful" });
            }
            else
            {
                // Kích hoạt không thành công
                return BadRequest(new { message = "Activation failed" });
            }
        }

        [HttpGet("info")]
        public IActionResult GetUserInfo ()
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    var user = _userService.GetUser(userID);
                    return Ok(user);

                }
                else
                {
                    return BadRequest(new { message = "Invalid UserId." });
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
