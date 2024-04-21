namespace AirPlane.Dto
{
    public class GetFlightsByDayRequest
    {
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportName { get; set; }
        public string? DepartureAirportName1 { get; set; }
        public string? ArrivalAirportName1 { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int Adults { get; set; }
        public int? Children { get; set; }
        public bool Roundtrip { get; set; }
    }
}
