using Data.Models;

namespace Login.Repo
{
    public interface IAcessTokenRepo
    {
        public AcessToken GetByAcessTokenByUserId(int userId);
    }
}
