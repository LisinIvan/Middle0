using Microsoft.EntityFrameworkCore;
using Middle0.Domain.Entities;
using Middle0.Persistence.Context;
using Middle0.Persistence.Repositories.Interfaces;

namespace Middle0.Persistence.Repositories
{
	public class EventRepository : IEventRepository
	{
		private EventDbContext _context;
		public EventRepository(EventDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AddEventEntity(Event entity)
		{
			Event eventEntity = await GetEventEntitiesByNameAsync(entity.Name);
			if (eventEntity == null)
			{
				var entry = _context.Entry(entity);
				Console.WriteLine(entry.State);
				await _context.Events.AddAsync(entity);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<bool> DeleteEventEntity(int eventId)
		{
			Event eventEntity = await _context.Events.FirstOrDefaultAsync(s => s.Id == eventId);
			if (eventEntity != null)
			{
				_context.Events.Remove(eventEntity);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
			//hrow new ExceptionDelete("Something wrong :)");
		}

		public async Task<List<Event>> GetAllEventEntitiesAsync()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task<bool> UpdateEventEntity(Event entity)
		{
			Event eventEntity = await _context.Events.FirstOrDefaultAsync(s => s.Id == entity.Id);
			if (eventEntity != null)
			{
				eventEntity.Name = entity.Name;
				eventEntity.Description = entity.Description;
				eventEntity.Category = entity.Category;
				eventEntity.Images = entity.Images;
				eventEntity.Place = entity.Place;
				eventEntity.Date = entity.Date;
				eventEntity.Time = entity.Time;
				eventEntity.AdditionalInfo = entity.AdditionalInfo;
				eventEntity.JobId = entity.JobId;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}
		public async Task<Event> GetEventEntitiesByNameAsync(string name)
		{
			Event eventEntity = await _context.Events.Where(c => c.Name == name).FirstOrDefaultAsync();
			return eventEntity;
		}
		public async Task<Event> GetEventById(int id)
		{
			Event eventEntity = await _context.Events.FirstOrDefaultAsync(s => s.Id == id);
			return eventEntity;
		}
	}
}
