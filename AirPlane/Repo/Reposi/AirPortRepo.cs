using AirPlane.Repo.IReposi;
using Data.DBContext;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AirPlane.Repo.Reposi
{
    public class AirPortRepo : IAirPortRepo
    {
        private readonly MyDb _myDb;

        public AirPortRepo(MyDb myDb)
        {
            _myDb = myDb;
        }

        public string GetAirportNameByIdAsync(int airportId)
        {
            var airport = _myDb.Airports
                .Where(a => a.AirportId == airportId)
                .Select(a => a.AirportName)
                .FirstOrDefault();

            return airport;
        }

        public List<Airport> GetLocaltion()
        {
            return _myDb.Airports.ToList();
        }
    }
}
