using System;
namespace mechat.messages
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public Type TypeInfo { get; set; }
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }

        public ChatMessage(){}

        public ChatMessage(string sender)
        {
            Id = Guid.NewGuid().ToString();
            TypeInfo = GetType();
            Sender = sender;
            Timestamp = DateTime.Now;
        }

    }
}
