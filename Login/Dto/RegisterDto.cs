using Data.Models.Enum;

namespace AirPlane.Dto
{
    public class RegisterDto
    {
        public string Email { set; get; }
        public string Password { set; get; }
        public string aliases { set; get; }
        public string Name { set; get; }
       
        public string Address { set; get; }
     
        public string Phone { set; get; }
    }
}
