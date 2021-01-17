using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RowCardGameEngine.ChatHub.Models;

namespace RowCardGameEngine.ChatHub
{
    public class ChatHub : Hub
    {
        private const int MaxMessageLength = 60;

        private readonly ChatThread _chatThread;

        public ChatHub(ChatThread chatThread)
        {
            _chatThread = chatThread;
        }

        public async Task SendMessage(string playerName, string message)
        {
            try
            {
                string cleanMessage =
                    Regex
                        .Replace(message, @"\s+", "");
                        
                string shortened =
                    cleanMessage.Substring(0, Math.Min(MaxMessageLength, cleanMessage.Length));

                _chatThread.AddMessage(new ChatMessage
                {
                    Message = shortened,
                    PlayerName = playerName,
                    CreatedAt = DateTime.Now
                });
            
                await this.Clients.All.SendAsync("NewMessage", playerName, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
