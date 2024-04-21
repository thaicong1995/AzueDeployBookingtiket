using AirPlane.Repo.IReposi;
using Data.DBContext;
using Data.Models;

namespace AirPlane.Repo.Reposi
{
    public class AirCraftRepo : IAirCraftRepo
    {
        private readonly MyDb _myDb;

        public AirCraftRepo(MyDb myDb)
        {
            _myDb = myDb;
        }
        public Aircraft GetAircraftById(int aircraftId)
        {
            return _myDb.Aircraft.FirstOrDefault(a => a.AircraftId == aircraftId);
        }
    }
}
