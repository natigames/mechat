using System;
using mechat.messages;

namespace mechat.core.EventHandlers
{
    public class MessageEventArgs
    {
        public ChatMessage Message { get; set; }
        public MessageEventArgs(ChatMessage message)
        {
            Message = message;
        }
    }
}
