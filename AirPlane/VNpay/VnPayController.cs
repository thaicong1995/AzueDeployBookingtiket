using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirPlane.VNpay
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly VNService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VnPayController(VNService vnPayService, IHttpContextAccessor httpContextAccessor)
        {
            _vnPayService = vnPayService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create-payment/{ticketNo}")]
        public IActionResult CreatePayment(string ticketNo)
        {
            try
            {
                var context = _httpContextAccessor.HttpContext;
                var paymentUrl = _vnPayService.CreatePaymentUrl(ticketNo, context);

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new { error = "cant create payment..", message = ex.Message });

            }
        }

        [HttpGet("payment-callback")]
        public IActionResult PaymentCallback([FromQuery] string vnp_OrderInfo, [FromQuery] string vnp_ResponseCode)
        {
            try
            {
                if (vnp_ResponseCode == "00")
                {
                    var resault = vnp_OrderInfo;


                    _vnPayService.UpdateTiketWhenSuccess(resault);
                    return Ok("success!!!");

                }

                else
                {
                    Console.WriteLine("Payment failed");
                    return BadRequest(new { error = "Payment failed." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "An error occurred", message = ex.Message });
            }
        }



        [HttpGet("create-payment")]
        public IActionResult CreatePaymentToken()
        {
            try
            {
                var context = _httpContextAccessor.HttpContext;
                var paymentUrl = _vnPayService.CreatePaymentUrlToken(context);

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new { error = "cant create payment..", message = ex.Message });
            }
        }


        [HttpGet("return-url")]
        public IActionResult ReturnUrl(string vnpResponseCode, string vnpTransactionNo)
        {
            // Kiểm tra mã phản hồi từ VNPAY
            if (vnpResponseCode == "00")
            {
                // Xử lý khi giao dịch thành công
                return Ok("Giao dịch thành công. Mã giao dịch: " + vnpTransactionNo);
            }
            else
            {
                // Xử lý khi giao dịch không thành công
                return BadRequest("Giao dịch không thành công. Mã phản hồi: " + vnpResponseCode);
            }
        }



    }
}
