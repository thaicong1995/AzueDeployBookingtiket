using Data.Models.Enum;
using MimeKit.Encodings;

namespace AirPlane.Dto
{
    public class FlightDto
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal EconomyFlexiblePrice { get; set; }
        public decimal BusinessFlexiblePrice { get; set; }
        public int RemainingEconomySeats { get; set; }
        public int RemainingBusinessSeats { get; set; }
        public AircraftDto Aircraft { get; set; }
        public RouteDto Route { get; set; }
        public FlightStatus Status { get; set; }

        public PriceDto price { get; set; }
        public List<SeastNumberResult> seastNumberResult { get; set; }
    }
}
