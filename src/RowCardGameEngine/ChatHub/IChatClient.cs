using System.Threading.Tasks;

namespace AzulGameEngine.ChatHub
{
    public interface IChatClient
    {
        Task SendNewPlayerMessage(string playerName);
    }
}