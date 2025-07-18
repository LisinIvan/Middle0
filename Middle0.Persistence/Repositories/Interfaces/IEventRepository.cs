using Middle0.Domain.Entities;

namespace Middle0.Persistence.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEventEntitiesAsync();
        Task<bool> AddEventEntity (Event entity);
        Task<bool> UpdateEventEntity (Event entity);
        Task<bool> DeleteEventEntity (int eventId);
        Task<Event> GetEventEntitiesByNameAsync(string name);
        Task<Event> GetEventById(int id);
    }
}
