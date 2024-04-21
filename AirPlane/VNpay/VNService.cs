using AirPlane.Repo.IReposi;
using AirPlane.Repo.Reposi;
using AirPlane.Service.IService;
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;
using Login.Helper;
using Org.BouncyCastle.Asn1.X509;
using SendMailAndPayMent.MailService;

namespace AirPlane.VNpay
{
    public class VNService
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ITicketService _ticketService;
        private readonly ITicketRepo _ticketRepo;
        private readonly MyDb _myDb;
        private readonly IFlightRepo _flightRepo;
        public VNService(ITicketService ticketService, IConfiguration configuration,
                                        ITicketRepo ticket, IFlightRepo flightRepo, MyDb myDb, EmailService emailService)
        {

            _myDb = myDb;
            _ticketService = ticketService;
            _configuration = configuration;
            _emailService = emailService;
            _ticketRepo = ticket;
            _flightRepo = flightRepo;
        }

        public string CreatePaymentUrl(string ticketNo, HttpContext context)
        {
            var isTicketExists = _ticketService.CheckTicket(ticketNo);
            if (isTicketExists)
            {
                var ticket = _ticketRepo.GetTicketByTicketNo(ticketNo);
                var pay = new VnPayLibrary();
                var timeNow = DateTime.Now;
                var tick = DateTime.Now.ToString();
                var amount = (int)ticket.AmountTotal * 100;
                var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
                pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
                pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
                pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
                pay.AddRequestData("vnp_Amount", amount.ToString());
                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_BankCode", "NCB");
                pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
                pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
                pay.AddRequestData("vnp_OrderInfo", $"{ticket.TicketNo}");
                Console.WriteLine($"TicketNo: {ticket.TicketNo}");
                pay.AddRequestData("vnp_OrderType", "other");
                pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
                pay.AddRequestData("vnp_TxnRef", tick);

                var paymentUrl =
                    pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

                return paymentUrl;
            }
            else
            {
                return "Can't Payment!";
            }
        }


        public void UpdateTiketWhenSuccess(string tiketNo)
        {
            var listTicket = _ticketRepo.GetListTicketByTicketNo(tiketNo);
            if (listTicket.Any())
            {
                foreach (var ticket in listTicket)
                {
                    ticket.statusTicket = StatusTicket.WaitFlight;
                    ticket.DateBooking = DateTime.Now;

                    _myDb.SaveChanges();

                    var flight = _flightRepo.GetFlightByFlightId(ticket.FlightId);

                    if (flight != null)
                    {
                        if (ticket.TypeSeats.ToLower() == "economy")
                        {
                            flight.RemainingEconomySeats--;
                        }
                        else if (ticket.TypeSeats.ToLower() == "business")
                        {
                            flight.RemainingBusinessSeats--;
                        }

                        _myDb.SaveChanges();

                        _emailService.SendTicketInfoEmail(ticket.Email, ticket);
                    }
                }
            }
            else
            {
                return;
            }
        }


        // Create  VNpayToken

        public string CreatePaymentUrlToken(HttpContext context)
        {
            var pay = new VnPayLibrary();
            var timeNow = DateTime.Now;
            var tick = DateTime.Now.ToString();
            var endPoint = "https://sandbox.vnpayment.vn/token_ui/create-token.html";
            // Thêm các tham số vào yêu cầu
            pay.AddRequestData("vnp_version", "2.1.0");
            pay.AddRequestData("vnp_command", "token_create");
            pay.AddRequestData("vnp_tmn_code", "TOKEN001");
            pay.AddRequestData("vnp_app_user_id", "1");
            pay.AddRequestData("vnp_bank_code", "VISA");
            pay.AddRequestData("vnp_locale", "vn");
            pay.AddRequestData("vnp_card_type", 01);
            pay.AddRequestData("vnp_txn_ref", tick);
            pay.AddRequestData("vnp_txn_desc", "Tao moi token");
            pay.AddRequestData("vnp_return_url", "https://localhost:7273/api/VnPay/return-url");
            pay.AddRequestData("vnp_cancel_url", "https://example.com/cancel");
            pay.AddRequestData("vnp_ip_addr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_create_date", timeNow.ToString("yyyyMMddHHmmss"));


            // Tạo URL yêu cầu

            var paymentUrl = pay.CreateRequestUrl1(endPoint, _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }






    }
}
