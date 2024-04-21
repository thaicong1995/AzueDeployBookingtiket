using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Aircraft
    {
        [Key]
        public int AircraftId { get; set; }
        public string Type { set; get; }
        public string AircraftCode { get; set; }
        public int TotalSeats { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
    }
}
