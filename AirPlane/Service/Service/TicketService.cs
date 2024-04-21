using AirPlane.Dto;
using AirPlane.Repo.IReposi;
using AirPlane.Repo.Reposi;
using AirPlane.Service.IService;
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;
using Login.Helper;
using System.Numerics;

namespace AirPlane.Service.Service
{
    public class TicketService : ITicketService
    {
        private readonly MyDb _myDb;
        private readonly ITicketRepo _ticketRepo;
        private readonly IFlightRepo _flightRepo;
        private readonly IPromotionRepo _promotionRepo;
        private readonly Token _token;
        public TicketService(MyDb myDb, ITicketRepo ticketRepo, IFlightRepo flightRepo, IPromotionRepo promotionRepo, Token token)
        {
            _flightRepo = flightRepo;
            _ticketRepo = ticketRepo;
            _myDb = myDb;
            _token = token;
            _promotionRepo = promotionRepo;
        }

        public string GenerateOrderNo()
        {
            DateTime now = DateTime.Now;
            string orderNo = $"ORD-{now:yyyyMMddHHmmss}";

            return orderNo;
        }
        public List<Ticket> Create(int flightId, int? flight1 , List<TicketRequest> oneWayTicketRequests, List<TicketRequest>? returnTicketRequests, int adults, int? children)
        {
            try
            {
                List<Ticket> tickets = new List<Ticket>();
                string oneWayTicketNo = GenerateOrderNo();
                decimal totalAmount = 0;
                string returnTicketNo = returnTicketRequests != null ? GenerateOrderNo() : null;

                foreach (var ticketRequest in oneWayTicketRequests)
                {
                    Ticket ticket = CreateTicketFromRequest(flightId,ticketRequest, oneWayTicketNo, adults, children);
                    tickets.Add(ticket);
                    _myDb.Add(ticket);
                    totalAmount += ticket.AmountTicket;
                }

                if (returnTicketRequests != null)
                {
                    foreach (var ticketRequest in returnTicketRequests)
                    {
                        Ticket ticket = CreateTicketFromRequest(flight1 ?? 0, ticketRequest, returnTicketNo, adults, children);
                        tickets.Add(ticket);
                        _myDb.Add(ticket);
                        totalAmount += ticket.AmountTicket;
                    }
                }

                // check ticket
                decimal totalDiscountAmount = oneWayTicketRequests
                   .Concat(returnTicketRequests ?? Enumerable.Empty<TicketRequest>()) 
                   .Where(ticketRequest => ticketRequest.PromotionId.HasValue)
                   .Select(ticketRequest =>
                   {
                       var promotion = _promotionRepo.GetPromotionById(ticketRequest.PromotionId.Value);
                       return promotion != null ? (totalAmount * (decimal)promotion.Value) : 0; 
                   })
                   .FirstOrDefault();

                totalAmount -= totalDiscountAmount;

                foreach (var ticket in tickets)
                {
                    ticket.AmountTotal = totalAmount;
                }


                _myDb.SaveChanges();

                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private Ticket CreateTicketFromRequest(int flightId, TicketRequest ticketRequest, string ticketNo, int adults, int? children)
        {

            var flight = _flightRepo.GetFlightForTicket(adults, children.GetValueOrDefault(), flightId, ticketRequest.TypeSeats);

            if(flight == null)
            {
                return null;
            }
            //decimal pricePerAdult = ticketRequest.TypeSeats.ToLower() == "economy" ? flight.EconomyFlexiblePrice : flight.BusinessFlexiblePrice;
            //decimal pricePerChild = ticketRequest.TypeSeats.ToLower() == "economy" ? flight.EconomyFlexiblePrice * 0.5m : flight.BusinessFlexiblePrice * 0.5m;
            decimal pricePerAdult = GetPricePerTicket("adult", ticketRequest.TypeSeats, flight);
            decimal pricePerChild = GetPricePerTicket("child", ticketRequest.TypeSeats, flight);
            decimal totalPrice = (pricePerAdult * adults) + (pricePerChild * children.GetValueOrDefault());

            return new Ticket
            {
                FlightId = flightId,
                TicketNo = ticketNo,
                FlightNumber = flight.FlightNumber,
                DepartureAirportName = flight.Route.DepartureAirport.AirportName,
                DepartureTime = flight.DepartureTime,
                ArrivalAirportName = flight.Route.ArrivalAirport.AirportName,
                ArrivalTime = flight.ArrivalTime,
                aliases = ticketRequest.aliases,
                FullName = ticketRequest.FullName,
                Email = ticketRequest.Email,
                Phone = ticketRequest.Phone,
                CustomType = ticketRequest.CustomType,
                PromotionId = ticketRequest.PromotionId,
                NumberSeats = ticketRequest.NumberSeats,
                TypeSeats = ticketRequest.TypeSeats,
                AmountTicket = ticketRequest.CustomType.ToLower() == "adult" ? pricePerAdult : pricePerChild,
               
                ticketType = ticketRequest.ticketType,
                DateBooking = DateTime.Now,
                statusTicket = StatusTicket.Pending,
            };
        }

        private decimal GetPricePerTicket(string customType, string seatClass, Flight flight)
        {
            // Xác định giá vé cho từng loại khách hàng và loại ghế
            if (customType.ToLower() == "adult")
            {
                return seatClass.ToLower() == "economy" ? flight.EconomyFlexiblePrice : flight.BusinessFlexiblePrice;
            }
            else if (customType.ToLower() == "child")
            {
                return seatClass.ToLower() == "economy" ? flight.EconomyFlexiblePrice * 0.5m : flight.BusinessFlexiblePrice * 0.5m;
            }
            else
            {
                throw new ArgumentException("CustomType is invalid.");
            }
        }
        public bool CheckTicket(string ticketNo)
        {
            var ticket = _ticketRepo.GetTicketByTicketNo(ticketNo);
            if (ticket != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Ticket> GetAllTicketForUser(string name, string email, string ticketNo)
        {
            try
            {
                var existTicket = _ticketRepo.GetTiketByTicketNoFlightId(name, email, ticketNo);
                if (existTicket == null)
                {
                    return null;
                }
                return existTicket;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Ticket Changeticket(int ticketid, int flightId, int adult, int? children, TicketRequest ticketrequest)
        {
            try
            {
                var existingticket = _ticketRepo.GetTiketById(ticketid);
                decimal totalAmount = 0;
                if (existingticket == null)
                {
                    throw new Exception("ticket not found.");
                }
               
                var today = DateTime.Now.Date;
                var departuredate = existingticket.DepartureTime.Date;
                var daysuntildeparture = (departuredate - today).Days;
                if (daysuntildeparture <= 3)
                {
                    throw new Exception("cannot change ticket within 3 days before departure.");
                }

                var oldflight = _flightRepo.GetFlightByFlightId(existingticket.FlightId);
                if (oldflight != null)
                {
                    if (existingticket.TypeSeats.ToLower() == "economy")
                    {
                        oldflight.RemainingEconomySeats++;
                    }
                    else if (existingticket.TypeSeats.ToLower() == "business")
                    {
                        oldflight.RemainingBusinessSeats++;
                    }
                    _myDb.SaveChanges();
                }

                existingticket.statusTicket = StatusTicket.Block;

                _myDb.SaveChanges();

                string ticketChange = GenerateOrderNo();
                Ticket newticket = CreateTicketFromRequest(flightId, ticketrequest, ticketChange, adult, children);
                totalAmount = newticket.AmountTicket;

                // Áp dụng giảm giá từ khuyến mãi nếu có
                if (ticketrequest.PromotionId.HasValue && ticketrequest.PromotionId > 0)
                {
                    var promotion = _promotionRepo.GetPromotionById(ticketrequest.PromotionId.Value);
                    if (promotion != null)
                    {
                        decimal discountAmount = (totalAmount * (decimal)promotion.Value);
                        totalAmount = totalAmount - discountAmount;
                    }
                }

                var newflight = _flightRepo.GetFlightByFlightId(flightId);
                if (newflight != null)
                {
                    if (ticketrequest.TypeSeats.ToLower() == "economy")
                    {
                        newflight.RemainingEconomySeats--;
                    }
                    else if (ticketrequest.TypeSeats.ToLower() == "business")
                    {
                        newflight.RemainingBusinessSeats--;
                    }
                }

                newticket.AmountTotal = totalAmount;
                
                _myDb.Add(newticket);
                _myDb.SaveChanges();

                return newticket;
            }
            catch (Exception ex)
            {
                throw new Exception("failed to change ticket. " + ex.Message);
            }
        }

        public string DeletedTicket(int ticketId)
        {
            try
            {
                var existingTicket = _ticketRepo.GetTiketById(ticketId);
                if (existingTicket == null)
                {
                    throw new Exception("Ticket not found.");
                }

                existingTicket.statusTicket = StatusTicket.Delete;
                _myDb.SaveChanges();
                return "deleted sucess!";

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
