using System;

namespace AzulGameEngine.ChatHub.Models
{
    public class ChatMessage
    {
        public string PlayerName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}