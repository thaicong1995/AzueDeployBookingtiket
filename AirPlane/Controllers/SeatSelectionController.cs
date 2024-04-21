//using AirPlane.Dto;
//using AirPlane.Service.Service;
//using Microsoft.AspNetCore.Mvc;

//namespace AirPlane.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class SeatSelectionController : ControllerBase
//    {
//        private readonly SeatService _seatService;

//        public FlightController(SeatService seatService)
//        {
//            _seatService = seatService;
//        }

//        [HttpPost("reserve-seat")]
//        public IActionResult ReserveSeat([FromBody] ReserveSeatRequest request)
//        {
//            _seatService.ReserveSeat(request.FlightNumber, request.SeatNumber);
//            return Ok();
//        }

//        [HttpGet("check-seat")]
//        public IActionResult CheckSeat(string flightNumber, string seatNumber)
//        {
//            bool isReserved = _seatService.IsSeatReserved(flightNumber, seatNumber);
//            return Ok(new { isReserved });
//        }
//    }

//    public class ReserveSeatRequest
//    {
//        public string FlightNumber { get; set; }
//        public string SeatNumber { get; set; }
//    }
//}
