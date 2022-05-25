using Microsoft.AspNetCore.SignalR;

namespace SameApp.Hubs;

public class ChatHub : Hub
{
    public async Task Changed(string message, string currentUser, string receiverContactId)
    {
        await Clients.All.SendAsync("ChangeReceived", message, currentUser);
    }
    
    public async Task AddContact(string displayName, string currentUser)
    {
        await Clients.All.SendAsync("ChangeContact", displayName, currentUser);
    }
    
    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
    }
    
}