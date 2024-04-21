using AirPlane.Dto;
using Login.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Login.Controllers
{
    public class DecodeController : ControllerBase
    {
        private readonly Token _token;

        public DecodeController(Token token)
        {

            _token = token;
        }


        [Authorize]
        [HttpGet("decode")]
        public ActionResult<UserInfo> DecodeToken()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized();
                }

                var authorizationHeader = HttpContext.Request.Headers["Authorization"];
                var token = authorizationHeader.ToString().Replace("Bearer ", "");

                var principal = _token.DecodeToken(token);

                if (principal == null)
                {
                    return BadRequest("Invalid token");
                }

                var userInfo = _token.GetUserInfoByToken(principal);

                if (userId != int.Parse(userIdClaim.Value))
                {
                    return Unauthorized("Fail token decode. (#user)");
                }

                return Ok(userInfo);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
