using AirPlane.Dto;
using AirPlane.Mapper;
using AirPlane.Repo.IReposi;
using AirPlane.Service.IService;
using Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace AirPlane.Service.Service
{
    public class FlightService : IFlightService
    {

        private readonly IFlightRepo _flightRepo;
        private readonly AirPlane.Mapper.FlightMapper _flightMapper;

        public FlightService(IFlightRepo flight, AirPlane.Mapper.FlightMapper flightMapper)
        {
            _flightRepo = flight;
            _flightMapper = flightMapper;
        }

        /// <summary>
        /// Get thông tin chuyến bay khi khách hàng chọn.
        /// </summary>
        /// <param name="departureAirportName"></param>
        /// <param name="arrivalAirportName"></param>
        /// <param name="departureAirportName1"></param>
        /// <param name="arrivalAirportName1"></param>
        /// <param name="departuredate"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="adults"></param>
        /// <param name="children"></param>
        /// <param name="flightNumber"></param>
        /// <param name="roundtrip"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public (FlightDto departureFlight, FlightDto returnFlight) SelectFlight(int adults, int? children,
                                                                        int flightId, int? flightId1, string seatClass, string seatClass1, bool roundtrip = false)
        {
            try
            {
                
                if (adults < 0 || (children.HasValue && children.Value < 0))
                {
                    throw new ArgumentException("Number of adults and children must be non-negative.");
                }

               

                var departureFlight = _flightRepo.SelectFlight( adults, children.GetValueOrDefault(),flightId, seatClass);
                 var returnFlight = roundtrip && flightId1.HasValue ? _flightRepo.SelectFlight(adults, children.GetValueOrDefault(), flightId1.Value, seatClass1) : null;

                if (departureFlight == null)
                    throw new Exception("No departure flight found matching the criteria.");

               

                var mappedDepartureFlight = _flightMapper.MapFlightSelect(departureFlight, seatClass, adults, children);
                var mappedReturnFlight = returnFlight != null ? _flightMapper.MapFlightSelect(returnFlight, seatClass1, adults, children) : null;

                return (mappedDepartureFlight, mappedReturnFlight);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        /// <summary>
        /// Get tất cả các chuyến bay trong ngày
        /// </summary>
        /// <param name="departureAirportName"></param>
        /// <param name="arrivalAirportName"></param>
        /// <param name="departureAirportName1"></param>
        /// <param name="arrivalAirportName1"></param>
        /// <param name="departuredate"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="adults"></param>
        /// <param name="children"></param>
        /// <param name="roundtrip"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public (List<FlightDto> departureFlights, List<FlightDto> returnFlights) GetFlightsByDate(string departureAirportName, string arrivalAirportName, 
                                                                    string? departureAirportName1, string? arrivalAirportName1, DateTime departuredate,
                                                                    DateTime? arrivaldate, int adults, int? children, bool roundtrip = false)
        {
            try
            {
                if (string.IsNullOrEmpty(departureAirportName) || string.IsNullOrEmpty(arrivalAirportName))
                {
                    throw new ArgumentException("Departure airport name, arrival airport name, and flight number are required.");
                }

                if (adults < 0 || (children.HasValue && children.Value < 0))
                {
                    throw new ArgumentException("Number of adults and children must be non-negative.");
                }

                if (arrivaldate.HasValue && arrivaldate.Value < DateTime.Now)
                {
                    throw new ArgumentException("Arrival time cannot be in the past.");
                }

                var departureFlights = _flightRepo.GetFlightsByDate(departureAirportName, arrivalAirportName, departuredate, adults, children.GetValueOrDefault());
                var returnFlights = roundtrip && arrivaldate.HasValue ?
                                        _flightRepo.GetFlightsByDate(departureAirportName1, arrivalAirportName1, arrivaldate.Value, adults, children.GetValueOrDefault()) :
                                        null;

                if (roundtrip && arrivaldate.HasValue && returnFlights == null)
                    throw new Exception("No return flight found matching the criteria.");

                var mappedDepartureFlights = !departureFlights.IsNullOrEmpty() ? _flightMapper.MapToFlightDto(departureFlights) : null;


                var mappedReturnFlights = !returnFlights.IsNullOrEmpty() ? _flightMapper.MapToFlightDto(returnFlights) : null;

                if (roundtrip && (mappedDepartureFlights == null || mappedReturnFlights == null))
                {
                    return (null, null);
                }

                return (mappedDepartureFlights, mappedReturnFlights);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
       

        /// <summary>
        /// Get all date > datetime. now
        /// </summary>
        /// <param name="departureAirportName"></param>
        /// <param name="arrivalAirportName"></param>
        /// <param name="departureAirportName1"></param>
        /// <param name="arrivalAirportName1"></param>
        /// <param name="roundtrip"></param>
        /// <returns></returns>
        public (List<FlightSearchResult> departureFlights, List<FlightSearchResult> returnFlights) SearchDateFlightsByRoute(string departureAirportName, string arrivalAirportName, string departureAirportName1, string arrivalAirportName1, bool roundtrip = false)
        {
            try
            {
                var departureFlights = _flightMapper.MapToFlightSearchResults(_flightRepo.GetFlightsByRouteAndDate(departureAirportName, arrivalAirportName));

                var returnFlights = roundtrip ? _flightMapper.MapToFlightSearchResults(_flightRepo.GetFlightsByRouteAndDate(departureAirportName1, arrivalAirportName1)) : new List<FlightSearchResult>();

                return (departureFlights, returnFlights);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
