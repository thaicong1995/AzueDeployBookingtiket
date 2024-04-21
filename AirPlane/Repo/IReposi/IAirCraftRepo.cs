using Data.Models;

namespace AirPlane.Repo.IReposi
{
    public interface IAirCraftRepo
    {
        public Aircraft GetAircraftById(int aircraftId);
    }
}
