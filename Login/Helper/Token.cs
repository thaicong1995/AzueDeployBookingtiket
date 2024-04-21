using AirPlane.Dto;
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Login.Helper
{
    public class Token
    {
        private readonly MyDb _myDb;
        private readonly IConfiguration _configuration;
        public Token(IConfiguration configuration, MyDb myDb)
        {
            _configuration = configuration;
            _myDb = myDb;
        }

        /// <summary>
        /// Tạo một JWT cho một người dùng đã được xác thực.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("MemberTypes", user.memberTypes.ToString()),
                new Claim("Aliases", user.aliases),
                new Claim("Name", user.Name.ToString()),
                new Claim(ClaimTypes.Role, user.roles.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token256").Value!)); // Sử dụng khóa 256 bit
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime now = DateTime.Now; 
            int expirationMinutes = 60; 
            DateTime expiration = now.AddMinutes(expirationMinutes); // Tính thời gian hết hạn

            var token = new JwtSecurityToken(claims: claims, expires: expiration,
                signingCredentials: cred, issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"]);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        /// <summary>
        ///  Kiểm tra và xác thực một chuỗi JWT.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token256"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        

        public ClaimsPrincipal DecodeToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token256").Value!));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var nameClaim = principal.FindFirst(ClaimTypes.Name);
                Console.WriteLine(principal);
                return principal;
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public UserInfo GetUserInfoByToken(ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var user = _myDb.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {

                    var aliasesClaim = principal.Claims.FirstOrDefault(c => c.Type == "Aliases");
                    var menberType = principal.Claims.FirstOrDefault(c => c.Type == "MemberTypes");
                    var firstNameClaim = principal.Claims.FirstOrDefault(c => c.Type == "Name");
                    var rolesClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                    if (aliasesClaim != null && firstNameClaim != null && rolesClaim != null)
                    {
                        var userInfo = new UserInfo
                        {
                            UserId = user.Id,
                            aliases = user.aliases,
                            Name = firstNameClaim.Value,
                            memberTypes = (MemberTypes)Enum.Parse(typeof(MemberTypes), menberType.Value),
                            roles = (Roles)Enum.Parse(typeof(Roles), rolesClaim.Value)
                        };

                        return userInfo;
                    }
                }
            }

            return null;
        }


    }
}
