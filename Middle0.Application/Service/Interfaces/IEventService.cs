

using Middle0.Domain.Entities;

namespace Middle0.Application.Service.Interfaces
{
    public interface IEventService
    {
		Task<List<Event>> GetAllEventAsync();
		Task<bool> AddEventEntity(Event entity);
		Task<bool> UpdateEventEntity(Event entity);
		Task<bool> DeleteEventEntity(int eventId);
		Task<Event> GetEventByNameAsync(string name);
		Task<Event> GetEventById(int id);
	}
}
