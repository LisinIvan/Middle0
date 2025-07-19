using Middle0.Domain.Entities;
using System.Net.Http.Json;
namespace Middle0.UI.Services
{
	public class EventServiceUi
	{
		private readonly HttpClient _httpClient;

		public EventServiceUi(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<List<Event>> GetAllEventAsync()
		{
			return await _httpClient.GetFromJsonAsync<List<Event>>("api/events");
		}
		public async Task<bool> CreateEventAsync(Event ev)
		{
			var response = await _httpClient.PostAsJsonAsync("api/events", ev);
			return response.IsSuccessStatusCode;
		}
		public async Task<bool> UpdateEventAsync(int id, Event ev)
		{
			var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", ev);
			return response.IsSuccessStatusCode;
		}
		public async Task<bool> DeleteEventAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/events/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
