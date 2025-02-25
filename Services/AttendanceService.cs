using EventEaseApp.Models;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventEaseApp.Services
{
    public class AttendanceService
    {
        private readonly IJSRuntime _jsRuntime;
        private List<RegistrationModel> registrations = new();

        public AttendanceService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task RegisterUser(RegistrationModel model)
        {
            await LoadRegistrations();
            if (!await IsUserRegistered(model.UserEmail, model.EventId))
            {
                registrations.Add(model);
                await SaveRegistrations();
            }
        }

        public async Task<bool> IsUserRegistered(string email, int eventId)
        {
            await LoadRegistrations();
            return registrations.Any(r => r.UserEmail == email && r.EventId == eventId);
        }

        public async Task<List<RegistrationModel>> GetEventAttendees(int eventId)
        {
            await LoadRegistrations();
            return registrations.Where(r => r.EventId == eventId).ToList();
        }

        public async Task RemoveAttendee(string email, int eventId)
        {
            await LoadRegistrations();
            registrations = registrations.Where(r => !(r.UserEmail == email && r.EventId == eventId)).ToList();
            await SaveRegistrations();
        }

        private async Task SaveRegistrations()
        {
            var json = JsonSerializer.Serialize(registrations);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "attendees", json);
        }

        private async Task LoadRegistrations()
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "attendees");
            if (!string.IsNullOrEmpty(json))
            {
                registrations = JsonSerializer.Deserialize<List<RegistrationModel>>(json) ?? new List<RegistrationModel>();
            }
        }
    }
}
