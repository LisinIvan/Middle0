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

		public async Task<bool> AddEventEntity(EventEntities entity)
		{
			EventEntities eventEntity = await GetEventEntitiesByNameAsync(entity.Name);
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
			EventEntities eventEntity = await _context.Events.FirstOrDefaultAsync(s => s.Id == eventId);
			if (eventEntity != null)
			{
				_context.Events.Remove(eventEntity);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
			//hrow new ExceptionDelete("Something wrong :)");
		}

		public async Task<List<EventEntities>> GetAllEventEntitiesAsync()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task<bool> UpdateEventEntity(EventEntities entity)
		{
			EventEntities eventEntity = await _context.Events.FirstOrDefaultAsync(s => s.Id == entity.Id);
			if (eventEntity != null)
			{
				eventEntity.Name = entity.Name;
				eventEntity.Description = entity.Description;
				eventEntity.Category = entity.Category;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}
		public async Task<EventEntities> GetEventEntitiesByNameAsync(string name)
		{
			EventEntities eventEntity = await _context.Events.Where(c => c.Name == name).FirstOrDefaultAsync();
			return eventEntity;
		}
		public async Task<EventEntities> GetEventById(int id)
		{
			EventEntities eventEntity = await _context.Events.FirstOrDefaultAsync(s => s.Id == id);
			return eventEntity;
		}
	}
}
