using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class PassengerTicket
    {
        [Key]
        public int Id { get; set; }

        // Thông tin vé của hành khách
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int SeatNumber { get; set; } 
        public int TicketId { get; set; } 
        public Ticket Ticket { get; set; } 
    }
}
