using Microsoft.AspNetCore.Mvc;
using Middle0.Application.Service.Interfaces;
using Middle0.Domain.Entities;

namespace Middle0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

		private readonly IEventEntitiesService _eventService;

		public EventsController(IEventEntitiesService eventService)
		{
			_eventService = eventService;
		}

		// GET: api/EventEntities
		[HttpGet]
		public async Task<ActionResult<List<EventEntities>>> GetAll()
		{
			var result = await _eventService.GetAllEventEntitiesAsync();
			return Ok(result);
		}

		// GET: api/EventEntities/name/SomeName
		[HttpGet("name/{name}")]
		public async Task<ActionResult<EventEntities>> GetByName(string name)
		{
			var result = await _eventService.GetEventEntitiesByNameAsync(name);
			if (result == null)
				return NotFound();
			return Ok(result);
		}

		// POST: api/EventEntities
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] EventEntities entity)
		{
			await _eventService.AddEventEntity(entity);
			return Ok();
		}

		// PUT: api/EventEntities
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] EventEntities entity)
		{
			await _eventService.UpdateEventEntity(entity);
			return Ok();
		}

		// DELETE: api/EventEntities/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _eventService.DeleteEventEntity(id);
			if (!result)
				return NotFound();
			return Ok();
		}
	}
}
