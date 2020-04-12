using System.Collections.Concurrent;
using AzulGameEngine.ChatHub.Models;

namespace AzulGameEngine.ChatHub
{
    public class ChatThread
    {
        private readonly ConcurrentQueue<ChatMessage> chatMessageQueue;

        public ChatThread()
        {
            chatMessageQueue = new ConcurrentQueue<ChatMessage>();
        }

        public void AddMessage(ChatMessage message)
        {
            chatMessageQueue.Enqueue(message);
        }

        public void Clear()
        {
            chatMessageQueue.Clear();
        }
    }
}