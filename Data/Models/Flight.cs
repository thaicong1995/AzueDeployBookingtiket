using Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public string FlightNumber { set; get; }
        public int RouteId { get; set; }
        public int AircraftId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal EconomyFlexiblePrice { get; set; } = 0;
        public decimal BusinessFlexiblePrice { get; set; } = 0;
        public int RemainingEconomySeats { get; set; }
        public int RemainingBusinessSeats { get; set; }
        public FlightStatus Status { get; set; }

        public Routes Route { get; set; }
        public Aircraft Aircraft { get; set; }
    
       
    }
}
