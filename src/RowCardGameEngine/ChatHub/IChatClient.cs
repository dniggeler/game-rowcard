using System.Threading.Tasks;

namespace RowCardGameEngine.ChatHub
{
    public interface IChatClient
    {
        Task SendNewPlayerMessage(string playerName);
    }
}