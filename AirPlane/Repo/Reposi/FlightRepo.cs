using AirPlane.Repo.IReposi;
using Data.DBContext;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AirPlane.Repo.Reposi
{
    public class FlightRepo : IFlightRepo
    {
        private readonly MyDb _myDb;

        public FlightRepo(MyDb myDb)
        {
            _myDb = myDb;
        }

        public Flight SelectFlight(int adults, int children, int flightId, string seatClass)
        {
            var totalPerson = adults + children;
            var selectedFlight = _myDb.Flights
                        .Include(f => f.Route)
                        .Include(f => f.Aircraft)
                        .FirstOrDefault(f => f.FlightId == flightId
                                    && (f.Aircraft.EconomySeats + f.Aircraft.BusinessSeats) >= totalPerson
                                    && (seatClass.ToLower() == "economy" ? f.Aircraft.EconomySeats > 0 : f.Aircraft.BusinessSeats > 0));



            return selectedFlight;
        }

        public List<Flight> GetFlightsByDate(string departureAirportName, string arrivalAirportName, DateTime date,
                                            int adults, int children)
        {
            var totalPerson = adults + children;
            var today = DateTime.Today;

            var isToday = date.Date == today;
            return _myDb.Flights
                 .Include(f => f.Aircraft)
                 .Include(f => f.Route)
                 .Where(f => f.Route.DepartureAirport.AirportName == departureAirportName
                         && f.Route.ArrivalAirport.AirportName == arrivalAirportName
                         && f.DepartureTime.Date == date.Date
                         && (isToday ? f.DepartureTime > DateTime.Now : true)
                         && (f.Aircraft.EconomySeats + f.Aircraft.BusinessSeats) >= totalPerson)
                 .ToList();
        }

        public List<Flight> GetFlightsByRouteAndDate(string departureAirportName, string arrivalAirportName)
        {
            return _myDb.Flights
                .Where(f => f.Route != null && f.Route.DepartureAirport != null && f.Route.ArrivalAirport != null
                        && f.Route.DepartureAirport.AirportName == departureAirportName
                        && f.Route.ArrivalAirport.AirportName == arrivalAirportName
                        && f.DepartureTime >= DateTime.Now)
                .GroupBy(f => new { f.DepartureTime.Date, f.EconomyFlexiblePrice }) // Group by date and price
                .Select(g => g.OrderBy(f => f.EconomyFlexiblePrice).First()) // Select the first flight in each group
                .ToList();
        }

        public Flight GetFlightByFlightId(int flightId)
        {
            return _myDb.Flights.FirstOrDefault(f => f.FlightId == flightId);
        }

        public Flight GetFlightForTicket(int adults, int children, int flightId, string seatClass)
        {
            var totalPerson = adults + children;
            var selectedFlight = _myDb.Flights
                .Include(f => f.Route)
                    .ThenInclude(r => r.DepartureAirport)
                .Include(f => f.Route)
                    .ThenInclude(r => r.ArrivalAirport)
                .Include(f => f.Aircraft)
                .FirstOrDefault(f => f.FlightId == flightId
                            && (f.Aircraft.EconomySeats + f.Aircraft.BusinessSeats) >= totalPerson
                            && (seatClass.ToLower() == "economy" ? f.Aircraft.EconomySeats > 0 : f.Aircraft.BusinessSeats > 0));

            return selectedFlight;
        }
    }
}
