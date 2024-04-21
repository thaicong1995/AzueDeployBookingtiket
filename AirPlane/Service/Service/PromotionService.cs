using AirPlane.Repo.IReposi;
using AirPlane.Service.IService;
using Data.Models;
using Data.Models.Enum;


namespace AirPlane.Service.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepo _promotionRepo;
        public PromotionService(IPromotionRepo promotionRepo)
        {
            _promotionRepo = promotionRepo;
        }
        public List<Promotion> GetAllPromotion(MemberTypes memberTypes)
        {
            try
            {
                var promotions = _promotionRepo.GetAllPromotion(memberTypes);
                return promotions;
            }
            catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }
    }
}
