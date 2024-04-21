using Data.Models;
using RestSharp;
using System.Text;

namespace SendMailAndPayMent.MailService
{
    public class EmailService
    {
        private readonly string apiKey;
        private readonly string domain;

        public EmailService()
        {
            apiKey = "";
            domain = "";
        }

        //Send MailGun 
        public bool SendActivationEmail(string toEmail, string activationLink)
        {
            try
            {
                var client = new RestClient("");
                var request = new RestRequest();
                request.Resource = $"{domain}/messages";
                request.AddParameter("from", "thaicong1995@gmail.com");
                request.AddParameter("to", toEmail);
                request.AddParameter("subject", "Activate Your Account");
                request.AddParameter("html", $"Click the link to activate your account: {activationLink}");
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey)));
                request.Method = Method.Post;

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    Console.WriteLine($"Password reset email sent to {toEmail} successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to send password reset email to {toEmail}. Error: {response.ErrorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending password reset email to {toEmail}. Error: {ex.Message}");
                return false;
            }
        }


        //Send MailGun 
        public bool SendTicketInfoEmail(string toEmail, Ticket ticket)
        {
            try
            {
                var client = new RestClient("");
                var request = new RestRequest();
                request.Resource = $"{domain}/messages";
                request.AddParameter("from", "thaicong1995@gmail.com");
                request.AddParameter("to", toEmail);
                request.AddParameter("subject", "Your Ticket");
                string ticketHtmlContent = $@"
                                <html>
                                <head>
                                    <style>
                                        /* Định nghĩa các quy tắc CSS */
                                        body {{
                                            font-family: Arial, sans-serif;
                                            background-color: #f2f2f2;
                                            margin: 0;
                                            padding: 0;
                                        }}
                                        .ticket {{
                                            width: 400px;
                                            margin: 20px auto;
                                            background-color: #fff;
                                            border: 2px solid #ccc;
                                            border-radius: 10px;
                                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                            padding: 20px;
                                        }}
                                        .ticket h1 {{
                                            color: #333;
                                            font-size: 24px;
                                            text-align: center;
                                        }}
                                        .ticket p {{
                                            margin: 10px 0;
                                        }}
                                        .ticket strong {{
                                            font-weight: bold;
                                        }}
                                        .ticket-info {{
                                            background-color: #f2f2f2;
                                            padding: 10px;
                                            border-radius: 5px;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='ticket'>
                                        <h1>Thông tin vé máy bay</h1>
                                        <div class='ticket-info'>
                                            <p><strong>Số vé:</strong> {ticket.TicketNo}</p>
                                            <p><strong>Số chuyến bay:</strong> {ticket.FlightNumber}</p>
                                            <p><strong>Sân bay đi:</strong> {ticket.DepartureAirportName}</p>
                                            <p><strong>Thời gian khởi hành:</strong> {ticket.DepartureTime}</p>
                                            <p><strong>Sân bay đến:</strong> {ticket.ArrivalAirportName}</p>
                                            <p><strong>Thời gian đến:</strong> {ticket.ArrivalTime}</p>
                                            <p><strong>Loại ghế:</strong> {ticket.TypeSeats}</p>
                                            <p><strong>Giá vé:</strong> {ticket.AmountTicket}</p>
                                            <p><strong>Tổng cộng:</strong> {ticket.AmountTotal}</p>
                                            <p><strong>Ngày đặt vé:</strong> {ticket.DateBooking}</p>
                                        </div>
                                    </div>
                                </body>
                                </html>";



                request.AddParameter("html", ticketHtmlContent);
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey)));
                request.Method = Method.Post;

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    Console.WriteLine($"Password reset email sent to {toEmail} successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to send password reset email to {toEmail}. Error: {response.ErrorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending password reset email to {toEmail}. Error: {ex.Message}");
                return false;
            }
        }
    }
}
