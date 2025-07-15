using Microsoft.AspNetCore.Mvc;
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;

namespace Middle0.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[ProducesResponseType(typeof(EventEntities), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class EventsController : Controller
	{

		private readonly IEventEntitiesService _eventService;
		private readonly ILogger<EventsController> _logger;

		public EventsController(IEventEntitiesService eventService, ILogger<EventsController> logger)
		{
			_eventService = eventService;
			_logger = logger;
		}

		// GET: api/EventEntities
		[HttpGet]
		[ProducesResponseType(typeof(List<EventEntities>), StatusCodes.Status200OK)]
		public async Task<ActionResult<List<EventEntities>>> GetAll()
		{
			var result = await _eventService.GetAllEventEntitiesAsync();
			return Ok(result);
		}

		//GET: api/EventEntities/{id}}
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(EventEntities), StatusCodes.Status200OK)]
		public async Task<ActionResult<EventEntities>> GetById(int id)
		{
			try
			{
				var result = _eventService.GetEventEntitiesById(id);
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

		// GET: api/EventEntities/name/SomeName
		[HttpGet("name/{name}")]
		[ProducesResponseType(typeof(EventEntities), StatusCodes.Status200OK)]
		public async Task<ActionResult<EventEntities>> GetByName(string name)
		{
			var result = await _eventService.GetEventEntitiesByNameAsync(name);
			if (result == null)
				return NotFound();
			return Ok(result);
		}

		// POST: api/EventEntities
		[HttpPost]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public async Task<IActionResult> Create([FromBody] EventEntities entity)
		{
			try
			{
				bool added = await _eventService.AddEventEntity(entity);

				if (added)
					return Ok(new { message = "Событие успешно создано" });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			return Conflict(new { message = "Событие с таким именем уже существует" });
		}

		// PUT: api/EventEntities
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Update([FromBody] EventEntities entity)
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

		// DELETE: api/EventEntities/{id}
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
