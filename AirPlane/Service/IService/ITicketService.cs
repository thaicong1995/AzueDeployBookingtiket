using AirPlane.Dto;
using Data.Models;

namespace AirPlane.Service.IService
{
    public interface ITicketService
    {
        public List<Ticket> Create(int flightId, int? flight1, List<TicketRequest> oneWayTicketRequests, List<TicketRequest>? returnTicketRequests, int adults, int? children);
        public bool CheckTicket(string ticketNo);

        public List<Ticket> GetAllTicketForUser(string name, string email, string ticketNo);
        public Ticket Changeticket(int ticketid, int flightId, int adult, int? children, TicketRequest ticketrequest);

        public string DeletedTicket(int ticketId);
    }
}
