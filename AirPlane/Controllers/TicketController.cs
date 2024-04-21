using AirPlane.Dto;
using AirPlane.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirPlane.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("createTicket")]
        public IActionResult CreateTicket([FromBody] TicketCreationRequest ticketCreationRequest, int adults, int? children, 
                                                        int flightId, int? flight1)
        {
            try
            {

                var ticket = _ticketService.Create(flightId, flight1, ticketCreationRequest.OneWayTicketRequests, ticketCreationRequest.ReturnTicketRequests,adults, children);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getAllTicket")]
        public IActionResult GetAllTicketbyUser([FromQuery] TicketInfomationRequest infomationRequest)
        {
            try
            {
                var tickets = _ticketService.GetAllTicketForUser(infomationRequest.name, infomationRequest.email, infomationRequest.ticketNo);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("changeTicket")]
        public IActionResult ChangeTicket(int ticketid, int flightId, int adult, int? children, TicketRequest ticketrequest)
        {
            try
            {
                var tickets = _ticketService.Changeticket(ticketid, flightId, adult, children, ticketrequest);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deleteTicket")]
        public IActionResult DeleteTicked(int ticketId)
        {
            try
            {
                var tickets = _ticketService.DeletedTicket(ticketId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

