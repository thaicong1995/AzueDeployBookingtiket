using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Airport
    {
        [Key]
        public int AirportId { get; set; }
        public string AirportName { get; set; }
        public string Location { get; set; }
    }
}
