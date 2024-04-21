using Data.Models;

namespace AirPlane.Repo.IReposi
{
    public interface IFlightRepo
    {
        public Flight SelectFlight(int adults, int children, int flightId, string seatClass);
        public List<Flight> GetFlightsByRouteAndDate(string departureAirportName, string arrivalAirportName);
        public List<Flight> GetFlightsByDate(string departureAirportName, string arrivalAirportName, DateTime date, int adults, int children);
        public Flight GetFlightByFlightId(int flightId);

        public Flight GetFlightForTicket(int adults, int children, int flightId, string seatClass);

    }
}
