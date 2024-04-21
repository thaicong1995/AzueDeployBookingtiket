using AirPlane.Repo.IReposi;
using AirPlane.Service.IService;
using Data.Models;

namespace AirPlane.Service.Service
{
    public class AirportService : IAirportService
    {
        private readonly IAirPortRepo _airPortRepo;
        public AirportService(IAirPortRepo airPortRepo)
        {
            _airPortRepo = airPortRepo; 
        }
        public List<Airport> GetAllLocaltion()
        {
            var localtion = _airPortRepo.GetLocaltion();
            return localtion;
        }
    }
}
