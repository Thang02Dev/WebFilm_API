using Microsoft.AspNetCore.SignalR;

namespace WebFilm_API.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendUserCount(int userCount)
        {
            await Clients.All.SendAsync("ReceiveUserCount", userCount);
        }
    }
}
