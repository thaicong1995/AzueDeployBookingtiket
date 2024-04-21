using AirPlane.Repo.IReposi;
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;

namespace AirPlane.Repo.Reposi
{
    public class TicketRepo : ITicketRepo
    {
        private readonly MyDb _myDb;
        public TicketRepo(MyDb myDb)
        {
            _myDb = myDb;
        }

        public List<Ticket> GetListTicketByTicketNo(string ticketNo)
        {
            return _myDb.Tickets
               .Where(t => t.TicketNo == ticketNo && t.statusTicket == StatusTicket.Pending)
               .ToList();
        }

        public List<string> GetSeastNumberByFlight(int flightId, string typeSeast)
        {
            return _myDb.Tickets
                .Where(t => t.FlightId == flightId && t.TypeSeats == typeSeast && (t.statusTicket == StatusTicket.WaitFlight || t.statusTicket == StatusTicket.Change))
                .Select(t => t.NumberSeats)
                .ToList();
        }

        public Ticket GetTicketByTicketNo(string ticketNo)
        {
            return _myDb.Tickets.FirstOrDefault(t => t.TicketNo == ticketNo);
        }

        public Ticket GetTiketById(int ticketId)
        {
            return _myDb.Tickets.FirstOrDefault(t => t.Id == ticketId && t.statusTicket == StatusTicket.WaitFlight);
        }

        public List<Ticket> GetTiketByTicketNoFlightId(string name, string email, string ticketNo)
        {
            return _myDb.Tickets.Where(t => t.FullName == name && t.Email == email && t.TicketNo == ticketNo && t.statusTicket == StatusTicket.WaitFlight).ToList();
        }
    }
}
