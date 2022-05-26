using Microsoft.AspNetCore.SignalR;

namespace SameApp.Hubs;

public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> Connections = new Dictionary<string, string>();

    public async Task Changed(string message, string currentUser, string receiverContactId)
    {
        var receiver = receiverContactId.Trim();
        var sender = currentUser.Trim();

        var receiverConnectionString = Connections[receiver];
        var senderConnectionString = Connections[sender];

        await  Clients.Client(receiverConnectionString).SendAsync("ChangeReceived", message, sender, receiver);
        await  Clients.Client(senderConnectionString).SendAsync("Update", message, currentUser);
        
        //await Clients.All.SendAsync("ChangeReceived", message, currentUser);
    }
    
    public async Task AddContact(string displayName, string currentUser)
    {
        await Clients.All.SendAsync("ChangeContact", displayName, currentUser);
    }
    
    public void Connect(string userNameId)
    {
        if (!Connections.ContainsKey(userNameId.Trim()))
        {
            Connections.Add(userNameId.Trim(), Context.ConnectionId);
        }
        else
        {
            Connections[userNameId.Trim()] = Context.ConnectionId;
        }
    }
    
}