using Microsoft.AspNetCore.SignalR;
using SameApp.Models;
using SameApp.Services;

namespace SameApp.Hubs;

public class ChatHub : Hub
{
    private static readonly Dictionary<string, string> Connections = new Dictionary<string, string>();
    private readonly ContactService _serviceContacts;
    
    public ChatHub(ContactService service1)
    {
        _serviceContacts = service1;
    }


    public async Task Changed(string message, string currentUser, string receiverContactId)
    {
        var receiver = receiverContactId.Trim();
        var sender = currentUser.Trim();

        var receiverConnectionString = Connections[receiver];
        var senderConnectionString = Connections[sender];

        Contact contact = await _serviceContacts.GetContact(receiver, sender);

        await  Clients.Client(receiverConnectionString).SendAsync("ChangeReceived", message, sender, receiver,contact.Server);
        
        await  Clients.Client(senderConnectionString).SendAsync("Update", message, sender, receiver, contact.Server);
        
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