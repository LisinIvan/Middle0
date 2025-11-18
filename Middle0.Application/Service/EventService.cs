
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;
using Middle0.Persistence.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Middle0.Domain.Common.DTO;
using Middle0.Application.Hangfire;

namespace Middle0.Application.Service
{
	public class EventService : IEventService
	{
		private IEventRepository _eventRepository;
		private readonly IMapper _mapper;
		HangfireEventTasks _hangfire;

		public EventService(IEventRepository eventRepository, IMapper mapper, HangfireEventTasks hangfire)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
			_hangfire = hangfire;
		}

		public async Task<bool> AddEventEntity(EventDTO entityDTO)
		{
			Event entity = _mapper.Map<Event>(entityDTO);
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
		}

		public async Task<bool> DeleteEventEntity(int eventId)
		{
			return await _eventRepository.DeleteEventEntity(eventId);
		}

		public async Task<List<EventDTO>> GetAllEventAsync()
		{
			List<Event> eventList = await _eventRepository.GetAllEventEntitiesAsync();
			List<EventDTO> resultDTO = new List<EventDTO>();
			EventDTO eDTO = null;
			foreach (var entity in eventList) {
				eDTO = _mapper.Map<EventDTO>(entity);
				eDTO.SendEmail = await _hangfire.GetDateJob(entity.JobId);
				resultDTO.Add(eDTO);
			}
			return resultDTO;
		}

		public async Task<EventDTO> GetEventByNameAsync(string name)
		{
			return _mapper.Map<EventDTO>(await _eventRepository.GetEventEntitiesByNameAsync(name));
		}
		public async Task<bool> UpdateEventEntity(EventDTO entityDTO)
		{
			Event entity = _mapper.Map<Event>(entityDTO);
			if (await _eventRepository.GetEventById(entity.Id) != null)
			{
				await _eventRepository.UpdateEventEntity(entity);
				return true;
			}
			return false;
		}
		public async Task<EventDTO> GetEventById(int id)
		{
			return _mapper.Map<EventDTO>(await _eventRepository.GetEventById(id));
		}
	}
}
