using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auqaponics2.Hubs
{

    public class ChatHub : Hub
    {
        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("Send", $"{Context.User.Identity.Name}: {message}");
        }

         public async Task RemoveFromGroup(string groupName)
         {
            await Clients.Group(groupName).SendAsync("Send", $"{Context.User.Identity.Name} has left the group {groupName}.");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            
         }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.User.Identity.Name} has joined the group {groupName}.");
        }

      
    }
}

