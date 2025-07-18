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
	public class EventController : Controller
	{

		private readonly IEventService _eventService;
		private readonly ILogger<EventController> _logger;

		public EventController(IEventService eventService, ILogger<EventController> logger)
		{
			_eventService = eventService;
			_logger = logger;
		}

		// GET: api/Event
		[HttpGet]
		[ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var result = await _eventService.GetAllEventAsync();
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error with GetAll");
				return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
			}
		}

		//GET: api/Event/{id}}
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
		public async Task<ActionResult<Event>> GetById(int id)
		{
			try
			{
				var result = _eventService.GetEventById(id);
				if (result == null)
					return NotFound($"EventEntity with ID {id} not found.");
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error with GetById, where id = {id}", id);
				return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
			}
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
			try
			{
				bool added = await _eventService.AddEventEntity(entity);

				if (added)
					return Ok();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			return Conflict(new { message = "Событие с таким именем уже существует" });
		}

		// PUT: api/Event
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Update([FromBody] Event entity)
		{
			//await _eventService.UpdateEventEntity(entity);
			//return Ok();
			try
			{
				var success = await _eventService.UpdateEventEntity(entity);

				if (!success)
					return NotFound($"Событие с ID {entity.Id} не найдено или не удалось обновить.");

				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
			}
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
