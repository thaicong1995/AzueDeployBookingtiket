using AirPlane.Dto;
using AirPlane.Repo.IReposi;
using Data.Models;

namespace AirPlane.Mapper
{
    public class FlightMapper
    {
        private readonly IAirPortRepo _airPortRepo;
        private readonly ITicketRepo _ticketRepo;
        public FlightMapper(IAirPortRepo airPortRepo, ITicketRepo ticket)
        {
            _airPortRepo = airPortRepo;
            _ticketRepo = ticket;
        }
        public List<FlightSearchResult> MapToFlightSearchResults(List<Flight> flights)
        {
            return flights.Select(f => new FlightSearchResult { Date = f.DepartureTime.Date, EconomyFlexiblePrice = f.EconomyFlexiblePrice }).ToList();
        }

        public List<FlightDto> MapToFlightDto(List<Flight> flights)
        {
            return flights.Select(f => MapFlightToDto(f)).ToList();
        }

        public FlightDto MapFlightSelect(Flight flight, string seatClass, int adults, int? children)
        {
            return MapSelectFlightToDto(flight, seatClass, adults, children);
        }

        private FlightDto MapFlightToDto(Flight flight)
        {
            var flightDto = new FlightDto
            {
                FlightId = flight.FlightId,
                FlightNumber = flight.FlightNumber,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                EconomyFlexiblePrice = flight.EconomyFlexiblePrice,
                BusinessFlexiblePrice = flight.BusinessFlexiblePrice,
                RemainingEconomySeats = flight.RemainingEconomySeats,
                RemainingBusinessSeats = flight.RemainingBusinessSeats,
                Status = flight.Status
            };

            if (flight.Aircraft != null)
            {
                flightDto.Aircraft = new AircraftDto
                {
                    AircraftId = flight.Aircraft.AircraftId,
                    Type = flight.Aircraft.Type,
                    AircraftCode = flight.Aircraft.AircraftCode,
                    TotalSeats = flight.Aircraft.TotalSeats,
                    EconomySeats = flight.Aircraft.EconomySeats,
                    BusinessSeats = flight.Aircraft.BusinessSeats
                };
            }

            if (flight.Route != null)
            {
                flightDto.Route = new RouteDto
                {
                    RouteId = flight.Route.RouteId,
                    DepartureAirportId = flight.Route.DepartureAirportId,
                    DepartureAirportName = _airPortRepo.GetAirportNameByIdAsync(flight.Route.DepartureAirportId),
                    ArrivalAirportId = flight.Route.ArrivalAirportId,
                    ArrivalAirportName = _airPortRepo.GetAirportNameByIdAsync(flight.Route.ArrivalAirportId),
                    DistanceInKm = flight.Route.DistanceInKm
                };
            }

            return flightDto;
        }


        private FlightDto MapSelectFlightToDto(Flight flight, string seatClass, int adults, int? children)
        {
            var flightDto = new FlightDto
            {
                FlightId = flight.FlightId,
                FlightNumber = flight.FlightNumber,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                EconomyFlexiblePrice = flight.EconomyFlexiblePrice,
                BusinessFlexiblePrice = flight.BusinessFlexiblePrice,
               
                Status = flight.Status
            };

            if (seatClass.ToLower() == "economy")
            {
                flightDto.RemainingEconomySeats = flight.RemainingEconomySeats;
            }
            else if (seatClass.ToLower() == "business")
            {
                flightDto.RemainingBusinessSeats = flight.RemainingBusinessSeats;
            }

            if (flight.Aircraft != null)
            {
                flightDto.Aircraft = new AircraftDto
                {
                    AircraftId = flight.Aircraft.AircraftId,
                    Type = flight.Aircraft.Type,
                    AircraftCode = flight.Aircraft.AircraftCode,
                    TotalSeats = flight.Aircraft.TotalSeats
                };

                // Set the seats based on the selected seat class
                if (seatClass.ToLower() == "economy")
                {
                    flightDto.Aircraft.EconomySeats = flight.Aircraft.EconomySeats;
                }
                else if (seatClass.ToLower() == "business")
                {
                    flightDto.Aircraft.BusinessSeats = flight.Aircraft.BusinessSeats;
                }
            }

            if (flight.Route != null)
            {
                flightDto.Route = new RouteDto
                {
                    RouteId = flight.Route.RouteId,
                    DepartureAirportId = flight.Route.DepartureAirportId,
                    DepartureAirportName = _airPortRepo.GetAirportNameByIdAsync(flight.Route.DepartureAirportId),
                    ArrivalAirportId = flight.Route.ArrivalAirportId,
                    ArrivalAirportName = _airPortRepo.GetAirportNameByIdAsync(flight.Route.ArrivalAirportId),
                    DistanceInKm = flight.Route.DistanceInKm
                };
            }

            if (seatClass.ToLower() == "economy")
            {
                flightDto.price = new PriceDto
                {
                    AdultPrice = flight.EconomyFlexiblePrice * adults,
                    ChildPrice = flight.EconomyFlexiblePrice * 0.5m * children.GetValueOrDefault(),
                    TotalPrice = (flight.EconomyFlexiblePrice * adults) + (flight.EconomyFlexiblePrice * 0.5m * children.GetValueOrDefault())
                };
            }
            else if (seatClass.ToLower() == "business")
            {
                flightDto.price = new PriceDto
                {
                    AdultPrice = flight.BusinessFlexiblePrice * adults,
                    ChildPrice = flight.BusinessFlexiblePrice * 0.5m * children.GetValueOrDefault(),
                    TotalPrice = (flight.BusinessFlexiblePrice * adults) + (flight.BusinessFlexiblePrice * 0.5m * children.GetValueOrDefault())
                };
            }

            if (seatClass.ToLower() == "economy")
            {
                flightDto.seastNumberResult = _ticketRepo.GetSeastNumberByFlight(flight.FlightId, "economy")
                    .Select(seatNumber => new SeastNumberResult { SeatNumber = seatNumber })
                    .ToList();
            }
            else if (seatClass.ToLower() == "business")
            {
                flightDto.seastNumberResult = _ticketRepo.GetSeastNumberByFlight(flight.FlightId, "business")
                    .Select(seatNumber => new SeastNumberResult { SeatNumber = seatNumber })
                    .ToList();
            }
            return flightDto;
        }
    }
}
