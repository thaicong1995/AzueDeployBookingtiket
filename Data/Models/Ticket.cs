using Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { set; get; }
        public int FlightId { set; get; }
        public string TicketNo { get; set; }
        public string FlightNumber { set; get; }
        public string DepartureAirportName { set; get; }
        public DateTime DepartureTime { set; get; }
        public string ArrivalAirportName { set; get; }
        public DateTime ArrivalTime { set; get; }
        public string aliases { set; get; }
        public string FullName {  get; set; }
        public string Email { set; get; }
        public string Phone {  set; get; }
        public string CustomType { set; get; }
        public int? PromotionId { set; get; }
        public string NumberSeats { set; get; }
        public string TypeSeats { set; get;}
        public decimal AmountTicket { set; get; }
        public decimal AmountTotal { set; get; }
        public string ticketType { set; get; }
        public DateTime DateBooking { set; get; }
        public StatusTicket statusTicket { set; get; }
        public Flight Flight { set; get; }

    }
}
