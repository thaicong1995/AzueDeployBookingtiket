using Data.Models;
using Data.Models.Enum;

namespace Login.Repo
{
    public interface ITokenService
    {
        public void CreateOrUpdateToken(User user);
        public StatusToken CheckTokenStatus(int userId);
    }
}
