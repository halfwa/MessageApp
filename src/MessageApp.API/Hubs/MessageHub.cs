using MessageApp.BLL.Models;
using Microsoft.AspNetCore.SignalR;

namespace MessageApp.API.Hubs
{
    /// <summary>
    /// The hub which Web clients connect to to receive Message updates.
    /// </summary>
    public sealed class MessageHub : Hub
    {
        // TODO: add roles and implement receiving exclusive message type
        public async Task BroadcastUpdates(Message message)
        {
            var clientsWithRole = Clients.Groups("PremiumSubscribers");

            await clientsWithRole.SendAsync("receiveMessageUpdates", message);
        }
    }
}
