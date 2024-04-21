namespace AirPlane.Dto
{
    public class SelectFlightRequest
    {  
        public int Adults { get; set; } = 1;
        public int? Children { get; set; }
        public int FlightId { get; set; }
        public int? FlightId1 { get; set; }
        public string SeatClass { get; set; }
        public string? SeatClass1 { get; set; }
        public bool Roundtrip { get; set; }
    }

}
