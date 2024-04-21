using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Caching.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        // Thông tin về chuyến bay
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        // Số ghế
        public string SeatNumber { get; set; }

        // Thời gian tạo cache
        public DateTime CacheTime { get; set; }
    }
}
