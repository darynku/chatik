using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace DarynChat.API.BaseChatHub;

public interface IChatCient
{
    public Task ReceiveMessage(string userName, string message);
}

public class ChatHub(IDistributedCache cache) : Hub<IChatCient>
{
    private readonly IDistributedCache _cache = cache;

    public async Task JoinToChat(UserConnection connection)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

            var stringConnection = JsonSerializer.Serialize(connection);
            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage("admin", $"{connection.UserName} join to chat");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SendMessage(string message)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection is not null)
        {
            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage(connection.UserName, message);
        }
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);
        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
        if (connection is not null)
        {
            await _cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage("admin", $"{connection.UserName} leave from chat");
        }
        await base.OnDisconnectedAsync(exception);
    }
}