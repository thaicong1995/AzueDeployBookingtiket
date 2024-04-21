using AirPlane.Dto;
using Data.Models;

namespace AirPlane.Service.IService
{
    public interface IFlightService
    {
        public(FlightDto departureFlight, FlightDto returnFlight) SelectFlight(int adults, int? children,
                                                                         int flightId, int? flightId1, string seatClass, string seatClass1, bool roundtrip = false);
        public (List<FlightSearchResult> departureFlights, List<FlightSearchResult> returnFlights) SearchDateFlightsByRoute(string departureAirportName,
                                                                        string arrivalAirportName, string departureAirportName1,
                                                                        string arrivalAirportName1, bool roundtrip = false);
        public (List<FlightDto> departureFlights, List<FlightDto> returnFlights) GetFlightsByDate(string departureAirportName, string arrivalAirportName, 
                                                                        string? departureAirportName1, string? arrivalAirportName1, DateTime departuredate,
                                                                        DateTime? arrivaldate, int adults, int? children, bool roundtrip = false);

    }
}
