namespace AirPlane.Dto
{
    public class RouteDto
    {
        public int RouteId { get; set; }
        public int DepartureAirportId { get; set; }
        public string DepartureAirportName { get; set; }
        public int ArrivalAirportId { get; set; }
        public string ArrivalAirportName { get; set; }
        public double DistanceInKm { get; set; }
    }
}
