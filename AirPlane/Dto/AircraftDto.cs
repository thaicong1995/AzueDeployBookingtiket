namespace AirPlane.Dto
{
    public class AircraftDto
    {
        public int AircraftId { get; set; }
        public string Type { get; set; }
        public string AircraftCode { get; set; }
        public int TotalSeats { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
    }
}
