using Data.Models;
using Data.Models.Enum;

namespace AirPlane.Service.IService
{
    public interface IPromotionService
    {
        public List<Promotion> GetAllPromotion(MemberTypes memberTypes);
    }
}
