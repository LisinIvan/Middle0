

using Middle0.Domain.Common.DTO;
using Middle0.Domain.Entities;

namespace Middle0.Application.Service.Interfaces
{
    public interface IEventService
    {
		Task<List<EventDTO>> GetAllEventAsync();
		Task<bool> AddEventEntity(EventDTO entity);
		Task<bool> UpdateEventEntity(EventDTO entity);
		Task<bool> DeleteEventEntity(int eventId);
		Task<EventDTO> GetEventByNameAsync(string name);
		Task<EventDTO> GetEventById(int id);
	}
}
