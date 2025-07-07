
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;
using Middle0.Persistence.Repositories.Interfaces;

namespace Middle0.Application.Service
{
	public class EventEntitiesService : IEventEntitiesService
	{
		private IEventRepository _eventRepository;

		public EventEntitiesService(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}

		public async Task AddEventEntity(EventEntities entity)
		{
			await _eventRepository.AddEventEntity(entity);
		}

		public async Task<bool> DeleteEventEntity(int eventId)
		{
			return await _eventRepository.DeleteEventEntity(eventId);
		}

		public async Task<List<EventEntities>> GetAllEventEntitiesAsync()
		{
			return await _eventRepository.GetAllEventEntitiesAsync();
		}

		public async Task<EventEntities> GetEventEntitiesByNameAsync(string name)
		{
			return await _eventRepository.GetEventEntitiesByNameAsync(name);
		}

		public async Task UpdateEventEntity(EventEntities entity)
		{
			await _eventRepository.UpdateEventEntity(entity);
		}
	}
}
