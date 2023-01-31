using Cwiczenia7.Models.DTO;

namespace Cwiczenia7.Services
{
    public interface IDbService
    {
        Task<IEnumerable<TripDTO>> GetTrips();
        Task<bool> RemoveClient(int id);
        Task<bool> AssignClientToTrip(int idTrip, UpdateClientDTO updateClient);
    }
}
