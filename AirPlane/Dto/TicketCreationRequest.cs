namespace AirPlane.Dto
{
    public class TicketCreationRequest
    {
        public List<TicketRequest> OneWayTicketRequests { get; set; }
        public List<TicketRequest> ReturnTicketRequests { get; set; }
    }
}
