using AirPlane.Service.IService;
using Data.Models.Enum;
using Login.Helper;
using Login.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AirPlane.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly ITokenService _token;
        public PromotionController(IPromotionService promotionService, ITokenService token)
        {
            _promotionService = promotionService;
            _token = token;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("getpromotion")]
        public IActionResult GetPromotion()
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var memberTypesClaim = HttpContext.User.FindFirst("MemberTypes");
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    if (Enum.TryParse<MemberTypes>(memberTypesClaim.Value, out var memberTypes))
                    {
                        var promotions = _promotionService.GetAllPromotion(memberTypes);

                        return Ok(promotions);
                    }
                    else
                    {
                        return BadRequest("Invalid value for MemberTypes claim.");
                    }
                }
                else
                {
                    return BadRequest("User ID is not available in the claims.");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
