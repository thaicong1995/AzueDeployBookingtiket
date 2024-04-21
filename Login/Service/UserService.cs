using AirPlane.Dto;
using Data.DBContext;
using Data.Models.Enum;
using Data.Models;
using Login.Helper;
using Login.Repo;
using SendMailAndPayMent.MailService;

namespace Login.Service
{
    public class UserService : IUserService
    {
        private readonly MyDb _myDb;
        private readonly IUserRepo _userRepo;
        private readonly EmailService _emailService;
        private readonly Token _token;
        private readonly ITokenService _tokenService;
        public UserService(MyDb myDb, IUserRepo userRepo, EmailService emailService, Token token, ITokenService tokenService)
        {
            _myDb = myDb;
            _userRepo = userRepo;
            _emailService = emailService;
            _token = token;
            _tokenService = tokenService;
        }

        /// <summary>
        /// check thời gian hiệu lực của token gửi kèm trong email
        /// activationToken > Now thì mail còn khả dụng.
        /// sau khi xử lý thành công thì ActivationToken = ""
        /// </summary>
        /// <param name="activationToken"></param>
        /// <returns></returns>
        public bool ActivateUser(string activationToken)
        {
            var user = _userRepo.GetUserByActivationToken(activationToken);

            if (user != null && user.ExpLink > DateTime.Now)
            {
                user.status = StatusUser.Active;
                user.ActivationToken = "";

                _myDb.SaveChanges();
                return true;
            }
            return false;
        }

        public string ChangePassword()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int userId)
        {
            try
            {
                var user = _userRepo.FindByID(userId);
                return user;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///Login - Tài khoản chưa được active ở register
        /// Người dùng đăng nhập sẽ gửi mail xác nhận để kích hoạt
        /// kích hoạt thành công người dùng đăng nhập bình thường
        /// </summary>
        public string Login(LoginDto loginDto)
        {
            try
            {
                var user = _userRepo.FindByEmail(loginDto.Email);

                if (user == null)
                    return "Wrong Email or Password!";

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Pasword, user.Password))
                    return ("Wrong Email or Password!");

                if (user.status == StatusUser.Inactive && user.roles == Roles.User)
                {
                    string activationToken = Guid.NewGuid().ToString();
                    user.ActivationToken = activationToken;
                    string activationLink = "https://localhost:7231/api/User/activate?activationToken=" + activationToken;

                    if (_emailService.SendActivationEmail(user.Email, activationLink))
                    {
                        user.ExpLink = DateTime.Now.AddMinutes(60);
                        _myDb.SaveChanges();
                    }
                    return "Check your Email for active";
                }

                if (user.status == StatusUser.Inactive && user.roles == Roles.ManagerMent)
                {
                    return "Contact the administrator to activate the account";
                }


                string jwtToken = _token.CreateToken(user);
                _tokenService.CreateOrUpdateToken(user);
                return jwtToken;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///Register - Sau khi tạo tài khoản, 
        ///email sẽ được gửi về email bạn đã đang ký để kích hoạt
        /// </summary>
        public string Register(RegisterDto userDto)
        {
            try
            {
                if (userDto == null)
                {
                    return "Not enough information";
                }
                var user = new User
                {
                    Email = userDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    aliases = userDto.aliases,
                    Name = userDto.Name,
                   
                    Address = userDto.Address,
                   
                    Phone = userDto.Phone,
                    status = StatusUser.Inactive,
                    memberTypes = MemberTypes.Normal,

                    roles = Roles.User
                };

                string activationToken = Guid.NewGuid().ToString();
                user.ActivationToken = activationToken;
                _myDb.Users.Add(user);
                _myDb.SaveChanges();

                string activationLink = "https://localhost:7231/api/User/activate?activationToken=" + activationToken;

                if (_emailService.SendActivationEmail(user.Email, activationLink))
                {
                    user.ExpLink = DateTime.Now.AddMinutes(60);
                    _myDb.SaveChanges();
                }
                return "Register Success! Please check your Email for activation";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Dang ky tai khoan cho admin
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string RegisterAdmin(RegisterDto userDto)
        {
            try
            {
                if (userDto == null)
                {
                    return "Not enough information";
                }
                var user = new User
                {
                    Email = userDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    aliases = userDto.aliases,
                    Name = userDto.Name,

                    Address = userDto.Address,

                    Phone = userDto.Phone,
                    status = StatusUser.Active,
                    memberTypes = MemberTypes.Normal,

                    roles = Roles.User
                };

                string activationToken = Guid.NewGuid().ToString();
                user.ActivationToken = activationToken;
                _myDb.Users.Add(user);
                _myDb.SaveChanges();
                return "Register Account Admin Success! ";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
