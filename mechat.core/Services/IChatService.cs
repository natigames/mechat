using System;
using System.Threading.Tasks;
using mechat.messages;

namespace mechat.core.Services
{
    public interface IChatService
    {
        bool isConnected { get; }
        string ConnectionToken { get; set; }
        Task InitAsync(string userId);
        Task DisconnectAsync();
        Task SendMessageAsync(ChatMessage message);
    }
}
