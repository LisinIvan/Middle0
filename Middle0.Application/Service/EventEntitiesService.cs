
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;
using Middle0.Persistence.Repositories;
using Middle0.Persistence.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Middle0.Application.Service
{
	public class EventEntitiesService : IEventEntitiesService
	{
		private IEventRepository _eventRepository;

		public EventEntitiesService(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}

		public async Task<bool> AddEventEntity(EventEntities entity)
		{
			var context = new ValidationContext(entity);
			var results = new List<ValidationResult>();

			bool isValid = Validator.TryValidateObject(entity, context, results, true);

			if (!isValid)
			{
				string errorMessages = string.Join("; ", results.Select(r => r.ErrorMessage));
				throw new ArgumentException("Validation failed: " + errorMessages);
			}

			var existing = await _eventRepository.GetEventEntitiesByNameAsync(entity.Name);
			if (existing != null)
				return false;

			await _eventRepository.AddEventEntity(entity);
			return true;

			//return await _eventRepository.AddEventEntity(entity);
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

		public async Task<bool> UpdateEventEntity(EventEntities entity)
		{
			if (_eventRepository.GetEventById(entity.Id) != null)
			{
				await _eventRepository.UpdateEventEntity(entity);
				return true;
			}
			return false;
		}
		public async Task<EventEntities> GetEventEntitiesById(int id)
		{
			return await _eventRepository.GetEventById(id);
		}
	}
}
