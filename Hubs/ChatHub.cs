using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace Facebook.Hubs
{
    public class ChatHub:Hub
    {
        //send notification to user (you have new messages)
        public async Task SendMessage(string sender, string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("ReceiveMessage", sender, message);
        }
        public async Task SendFriendRequest(string sender, string receiver)
        {
           //notification for friendShip
            await Clients.User(receiver).SendAsync("ReceiveFriendRequest", sender);
        }

    }
}
