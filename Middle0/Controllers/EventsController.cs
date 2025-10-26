using Microsoft.AspNetCore.Mvc;
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;
using Middle0.Domain.Common.DTO;
using Middle0.Hangfire;

namespace Middle0.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	[ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class EventsController : Controller
	{

		private readonly IEventService _eventService;
		private readonly ILogger<EventsController> _logger;
		HangfireEventTasks _hangfire = new HangfireEventTasks();

		public EventsController(IEventService eventService, ILogger<EventsController> logger)
		{
			_eventService = eventService;
			_logger = logger;
		}

		// GET: api/Event
		[HttpGet]
		[ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{

			var result = await _eventService.GetAllEventAsync();
			return Ok(result);
		}

		//GET: api/Event/{id}}
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
		public async Task<ActionResult<Event>> GetById(int id)
		{
			var result = await _eventService.GetEventById(id);
			if (result == null)
				return NotFound($"EventEntity with ID {id} not found.");
			return Ok(result);


		}

		// GET: api/Event/name/SomeName
		[HttpGet("name/{name}")]
		[ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
		public async Task<ActionResult<Event>> GetByName(string name)
		{
			var result = await _eventService.GetEventByNameAsync(name);
			if (result == null)
				return NotFound();
			return Ok(result);
		}

		// POST: api/Event
		[HttpPost]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public async Task<IActionResult> Create([FromBody] EventEmailDTO entity)
		{
			Event e = new Event();
			e.Id = entity.Id;
			e.Category = entity.Category;
			e.Name = entity.Name;
			e.Images = entity.Images;
			e.Description = entity.Description;
			e.Place = entity.Place;
			e.Date = entity.Date;
			e.Time = entity.Time;
			e.AdditionalInfo = entity.AdditionalInfo;

			bool added = await _eventService.AddEventEntity(e);

			if (added)
			{
				await _hangfire.EventEmail(entity);
				return Ok();
			}

			return Conflict(new { message = "Событие с таким именем уже существует" });
		}

		// PUT: api/Event
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Update(int id, [FromBody] Event entity)
		{

			var success = await _eventService.UpdateEventEntity(entity);

			if (!success)
				return NotFound($"Событие с ID {entity.Id} не найдено или не удалось обновить.");

			return NoContent();

		}

		// DELETE: api/Event/{id}
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _eventService.DeleteEventEntity(id);
			if (!result)
				return NotFound();
			return Ok();
		}
	}
}
