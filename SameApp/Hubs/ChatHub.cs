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
        
        await  Clients.Client(senderConnectionString).SendAsync("Update", message, sender);
        
        //await Clients.All.SendAsync("ChangeReceived", message, currentUser);
    }
    
    public async Task AddContact(string displayName, string currentUser, string newUserName, string newServer)
    {
        currentUser = currentUser.Trim();

        if (currentUser != newUserName)
        {
            // who send the request for adding the contact.
            var senderConnectionString = Connections[currentUser];
            await  Clients.Client(senderConnectionString).SendAsync("ChangeContact", displayName, currentUser, newUserName, newServer);
        
            // the other user.
            var receiverConnectionString = Connections[newUserName];
            await  Clients.Client(receiverConnectionString).SendAsync("UpdateContact", displayName, currentUser);

        }
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