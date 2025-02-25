using Microsoft.JSInterop;
using EventEaseApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventEaseApp.Services
{
    public class EventService
    {
        private readonly IJSRuntime _jsRuntime;
        private List<EventModel> events = new();

        public EventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task Initialize()
        {
            var savedEventsJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "events");
            if (!string.IsNullOrEmpty(savedEventsJson))
            {
                events = JsonSerializer.Deserialize<List<EventModel>>(savedEventsJson) ?? new List<EventModel>();
            }
            else
            {
                events = new()
                {
                    new EventModel { Id = 1, Name = "Tech Conference", Date = new DateTime(2025, 3, 15), Location = "New York" },
                    new EventModel { Id = 2, Name = "Music Festival", Date = new DateTime(2025, 4, 10), Location = "Los Angeles" },
                    new EventModel { Id = 3, Name = "Startup Summit", Date = new DateTime(2025, 5, 20), Location = "San Francisco" }
                };
                await SaveEvents();
            }
        }

        public List<EventModel> GetEvents() => events;

        public EventModel? GetEventById(int id) => events.FirstOrDefault(e => e.Id == id);

        public async Task AddEvent(EventModel newEvent)
        {
            newEvent.Id = events.Count > 0 ? events.Max(e => e.Id) + 1 : 1;
            events.Add(newEvent);
            await SaveEvents();
        }

        private async Task SaveEvents()
        {
            var json = JsonSerializer.Serialize(events);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "events", json);
        }
    }
}
