

using Middle0.Domain.Entities;

namespace Middle0.Application.Service.Interfaces
{
    public interface IEventEntitiesService
    {
		Task<List<EventEntities>> GetAllEventEntitiesAsync();
		Task AddEventEntity(EventEntities entity);
		Task<bool> UpdateEventEntity(EventEntities entity);
		Task<bool> DeleteEventEntity(int eventId);
		Task<EventEntities> GetEventEntitiesByNameAsync(string name);

	}
}
