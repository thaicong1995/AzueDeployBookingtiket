using AirPlane.Repo.IReposi;
using Data.DBContext;
using Data.Models;
using Data.Models.Enum;

namespace AirPlane.Repo.Reposi
{
    public class PromotionRepo : IPromotionRepo
    {
        private readonly MyDb _myDb;
        public PromotionRepo(MyDb myDb)
        {
            _myDb = myDb;
        }
        public List<Promotion> GetAllPromotion(MemberTypes memberTypes)
        {
            return _myDb.Promotions.Where(p => p.ApplicableMemberType.Equals(memberTypes)).ToList();
        }

        public Promotion GetPromotionById(int Id)
        {
            return _myDb.Promotions.FirstOrDefault(p => p.Id == Id);
        }
    }
}
