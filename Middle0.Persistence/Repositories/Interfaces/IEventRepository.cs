using Middle0.Domain.Entities;

namespace Middle0.Persistence.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<List<EventEntities>> GetAllEventEntitiesAsync();
        Task AddEventEntity (EventEntities entity);
        Task UpdateEventEntity (EventEntities entity);
        Task<bool> DeleteEventEntity (int eventId);
        Task<EventEntities> GetEventEntitiesByNameAsync(string name);
    }
}
