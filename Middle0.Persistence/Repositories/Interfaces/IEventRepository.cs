using Middle0.Domain.Entities;

namespace Middle0.Persistence.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<List<EventEntities>> GetAllEventEntitiesAsync();
        Task<bool> AddEventEntity (EventEntities entity);
        Task<bool> UpdateEventEntity (EventEntities entity);
        Task<bool> DeleteEventEntity (int eventId);
        Task<EventEntities> GetEventEntitiesByNameAsync(string name);
        Task<EventEntities> GetEventById(int id);
    }
}
