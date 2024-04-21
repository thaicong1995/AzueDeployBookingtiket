using Data.Models;

namespace AirPlane.Repo.IReposi
{
    public interface ITicketRepo
    {
        public Ticket GetTicketByTicketNo(string ticketNo);
        public List<Ticket> GetListTicketByTicketNo(string ticketNo);

        public List<string> GetSeastNumberByFlight(int flightId, string typeSeast);

        public List<Ticket> GetTiketByTicketNoFlightId(string name, string email, string flightNumber);

        public Ticket GetTiketById(int ticketId);
    }
}
