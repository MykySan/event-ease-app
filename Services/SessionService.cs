using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace EventEaseApp.Services
{
    public class SessionService
    {
        private readonly IJSRuntime _jsRuntime;

        public SessionService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveUserSession(string userEmail)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "user_email", userEmail);
        }

        public async Task<string?> GetUserSession()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user_email");
        }

        public async Task ClearSession()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user_email");
        }
    }
}
