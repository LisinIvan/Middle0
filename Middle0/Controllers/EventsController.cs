using Microsoft.AspNetCore.Mvc;
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;

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
			var result = _eventService.GetEventById(id);
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
		public async Task<IActionResult> Create([FromBody] Event entity)
		{

			bool added = await _eventService.AddEventEntity(entity);

			if (added)
				return Ok();

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
