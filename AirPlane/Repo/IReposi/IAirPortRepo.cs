using Data.Models;

namespace AirPlane.Repo.IReposi
{
    public interface IAirPortRepo
    {
        string GetAirportNameByIdAsync(int airportId);
        List<Airport> GetLocaltion();
    }
}
