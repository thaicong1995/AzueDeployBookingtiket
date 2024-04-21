
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;
using System.Diagnostics;

namespace Login.Repo
{
    public class AcessTokenRepo : IAcessTokenRepo
    {
        private readonly MyDb _myDb;
        public AcessTokenRepo (MyDb myDb)
        {
            _myDb = myDb;
        }
        public AcessToken GetByAcessTokenByUserId(int userId)
        {
            return _myDb.AcessTokens.FirstOrDefault(a => a.UserID == userId && a.statusToken == StatusToken.Valid);
        }
    }
}
