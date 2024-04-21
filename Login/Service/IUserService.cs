using AirPlane.Dto;
using Data.Models;

namespace Login.Service
{
    public interface IUserService
    {
        public string Register(RegisterDto userDto);
        public string RegisterAdmin(RegisterDto userDto);
        public User GetUser(int userId);
        public string Login(LoginDto loginDto);
        public string ChangePassword();
        public bool ActivateUser(string activationToken);
    }
}
