
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;
using Login.Helper;

namespace Login.Repo
{
    public class TokenService : ITokenService
    {
        private readonly IAcessTokenRepo _acessTokenRepo;
        private readonly Token _token;
        private readonly MyDb _myDb;
        public TokenService(IAcessTokenRepo acessTokenRepo, Token token, MyDb myDb)
        {
            _myDb = myDb;
            _token = token;
            _acessTokenRepo = acessTokenRepo;
        }

        /// <summary>
        /// Khi login token sẽ được lưu về db theo userId
        /// Nếu token đã tồn tại sẽ được ghi đè lên token cũ
        /// Nếu chưa có thì tạo 1 bản ghi mới
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="Exception"></exception>
        public void CreateOrUpdateToken(User user)
        {
            try
            {
                var existToken = _acessTokenRepo.GetByAcessTokenByUserId(user.Id);
                if (existToken != null)
                {
                    var token = _token.CreateToken(user);

                    if (string.IsNullOrEmpty(token))
                        throw new Exception("Failed to create a token.");

                    existToken.AccessToken = token;
                    existToken.ExpirationDate = DateTime.Now.AddMinutes(60);

                }
                else
                {
                    var token = _token.CreateToken(user);

                    if (string.IsNullOrEmpty(token))
                        throw new Exception("Failed to create a token.");

                    var accessToken = new AcessToken
                    {
                        UserID = user.Id,
                        AccessToken = token,
                        statusToken = StatusToken.Valid,
                        ExpirationDate = DateTime.Now.AddMinutes(60)//--------------
                    };

                    _myDb.AcessTokens.Add(accessToken);

                }
                _myDb.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái của một token dựa trên User ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public StatusToken CheckTokenStatus(int userId)
        {
            // Tìm Giá trị của token theo UserId
            var userTokens = _myDb.AcessTokens
                .Where(t => t.UserID == userId && t.statusToken == StatusToken.Valid)
                .OrderByDescending(t => t.ExpirationDate)
                .FirstOrDefault();

            if (userTokens != null)
            {
                if (userTokens.ExpirationDate > DateTime.Now)
                {
                    return StatusToken.Valid;
                }
            }

            return StatusToken.Expired;
        }
    }
}
