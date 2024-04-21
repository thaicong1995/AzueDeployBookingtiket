using Data.Models;
using Data.Models.Enum;

namespace AirPlane.Repo.IReposi
{
    public interface IPromotionRepo
    {
        public List<Promotion> GetAllPromotion(MemberTypes memberTypes);
        public Promotion GetPromotionById(int Id);
    }
}
