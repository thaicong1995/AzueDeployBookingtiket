namespace AirPlane.Dto
{
    public class TicketRequest
    {  
        public string aliases { set; get; }
        public string FullName { get; set; }
        public string Email { set; get; }
        public string Phone { set; get; }

        public string CustomType { set; get; }
        public int? PromotionId { set; get; }
        public string NumberSeats { set; get; }
        public string TypeSeats { set; get; }
       
        public string ticketType { set; get; }
    }
}
