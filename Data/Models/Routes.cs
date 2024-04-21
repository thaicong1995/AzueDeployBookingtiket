using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Routes
    {
        [Key]
        public int RouteId { get; set; }
        public int DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }
        public int ArrivalAirportId { get; set; }
        public Airport ArrivalAirport { get; set; }

        public double DistanceInKm { get; set; }
    }
}
