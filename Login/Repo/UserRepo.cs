using Data.DBContext;
using Data.Models;

namespace Login.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly MyDb _myDb;
        public UserRepo(MyDb myDb)
        {
            _myDb = myDb;
        }
        public User FindByAll(int UserId)
        {
            throw new NotImplementedException();
        }

        public User FindByEmail(string Email)
        {
            return _myDb.Users.SingleOrDefault(u => u.Email == Email);
        }

        public User FindByID(int UserId)
        {
            return _myDb.Users.FirstOrDefault(u => u.Id == UserId);
        }

        public User GetUserByActivationToken(string activationToken)
        {
            return _myDb.Users.SingleOrDefault(u => u.ActivationToken == activationToken);
        }
    }
}
