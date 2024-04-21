using AirPlane.Dto;
using AirPlane.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AirPlane.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("search")]
        public IActionResult SearchFlights([FromQuery] SearchFlightsRequest request)
        {
            try
            {
                var (departureFlights, returnFlights) = _flightService.SearchDateFlightsByRoute(
                    request.DepartureAirportName, request.ArrivalAirportName,
                    request.DepartureAirportName1, request.ArrivalAirportName1,
                    request.Roundtrip);

                var response = new
                {
                    DepartureFlights = departureFlights,
                    ReturnFlights = returnFlights
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getByDate")]
        public IActionResult GetFlightsByDay([FromQuery] GetFlightsByDayRequest request)
        {
            try
            {
                var (departureFlights, returnFlights) = _flightService.GetFlightsByDate(
                    request.DepartureAirportName, request.ArrivalAirportName,
                    request.DepartureAirportName1, request.ArrivalAirportName1,
                    request.DepartureDate, request.ArrivalDate,
                    request.Adults, request.Children, request.Roundtrip);

                var response = new
                {
                    DepartureFlights = departureFlights,
                    ReturnFlights = returnFlights
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("select-Flight")]
        public IActionResult SelectFlight([FromQuery] SelectFlightRequest request)
        {
            try
            {
                var (departureFlights, returnFlights) = _flightService.SelectFlight(
                    request.Adults, request.Children,
                    request.FlightId, request.FlightId1,
                    request.SeatClass,request.SeatClass1, request.Roundtrip);

                var response = new
                {
                    DepartureFlights = departureFlights,
                    ReturnFlights = returnFlights
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
