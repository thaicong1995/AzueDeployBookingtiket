namespace AirPlane.Dto
{
    public class SearchFlightsRequest
    {
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportName { get; set; }
        public string? DepartureAirportName1 { get; set; }
        public string? ArrivalAirportName1 { get; set; }
        public bool Roundtrip { get; set; }
    }
}
