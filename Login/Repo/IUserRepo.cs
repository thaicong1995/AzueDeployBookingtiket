using Data.Models;

namespace Login.Repo
{
    public interface IUserRepo
    {
        public User FindByID(int UserId);
        public User FindByEmail(string Email);
        public User FindByAll(int UserId);
        public User GetUserByActivationToken(string activationToken);
    }
}
