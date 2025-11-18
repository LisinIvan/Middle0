using Microsoft.AspNetCore.Mvc;
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;
using Middle0.Domain.Common.DTO;
using Middle0.Application.Hangfire;

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
		HangfireEventTasks _hangfire;

		public EventsController(IEventService eventService, ILogger<EventsController> logger, HangfireEventTasks hangfire)
		{
			_eventService = eventService;
			_logger = logger;
			_hangfire = hangfire;
		}

		// GET: api/Event
		[HttpGet]
		[ProducesResponseType(typeof(List<EventDTO>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{

			var result = await _eventService.GetAllEventAsync();
			return Ok(result);
		}

		//GET: api/Event/{id}}
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(EventDTO), StatusCodes.Status200OK)]
		public async Task<ActionResult<EventDTO>> GetById(int id)
		{
			var result = await _eventService.GetEventById(id);
			if (result == null)
				return NotFound($"EventEntity with ID {id} not found.");
			return Ok(result);
		}

		// GET: api/Event/name/SomeName
		[HttpGet("name/{name}")]
		[ProducesResponseType(typeof(EventDTO), StatusCodes.Status200OK)]
		public async Task<ActionResult<EventDTO>> GetByName(string name)
		{
			var result = await _eventService.GetEventByNameAsync(name);
			if (result == null)
				return NotFound();
			return Ok(result);
		}

		// POST: api/Event
		[HttpPost]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		public async Task<IActionResult> Create([FromBody] EventDTO entityDTO)
		{
			bool added = await _eventService.AddEventEntity(entityDTO);
			if (added)
			{
				if (entityDTO.SendEmail != null)
				{
					string jobId = await _hangfire.EventEmail(entityDTO);

					EventDTO updateEventDTO = await _eventService.GetEventByNameAsync(entityDTO.Name);
					updateEventDTO.JobId = jobId;
					await _eventService.UpdateEventEntity(updateEventDTO);
				}
				return Ok();
			}
			return Conflict(new { message = "Событие с таким именем уже существует" });
		}

		// PUT: api/Event
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Update(int id, [FromBody] EventDTO entityDTO)
		{
			if (entityDTO.JobId != null)
			{
				if (entityDTO.SendEmail != await _hangfire.GetDateJob(entityDTO.JobId))
					entityDTO.JobId = await _hangfire.UpdateDateSendEmail(entityDTO);
			}
			else
			{
				if (entityDTO.SendEmail != null)
					entityDTO.JobId = await _hangfire.EventEmail(entityDTO);
			}

			var success = await _eventService.UpdateEventEntity(entityDTO);

			if (!success)
				return NotFound($"Событие с ID {entityDTO.Id} не найдено или не удалось обновить.");
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
