using System;
using System.Threading.Tasks;
using mechat.core.EventHandlers;
using mechat.core.Services;
using mechat.messages;

namespace mechat.client
{
    class Program
    {
        static ChatService service;
        static string userName;

        static async Task Main(string[] args)
        {
            Console.WriteLine("User Name:");
            userName = Console.ReadLine();

            service = new ChatService();
            service.OnReceivedMessage += Service_OnReceivedMessage;


            await service.InitAsync(userName);

            Console.WriteLine("You are now connected");

            var keepGoing = true;
            do
            {
                var text = Console.ReadLine();
                if(text == "exit")
                {
                    await service.DisconnectAsync();
                    keepGoing = false;
                }
                else
                {
                    var message = new SimpleTextMessage(userName)
                    {
                        Text = text
                    };
                await service.SendMessageAsync(message);
                }

            } while (keepGoing);
        }

        private static void Service_OnReceivedMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Sender == userName)
                return;
            if (e.Message.TypeInfo.Name == nameof(SimpleTextMessage))
            {
                var simpleText =
                       e.Message as SimpleTextMessage;

                var message = $"{simpleText.Sender}: {simpleText.Text}";
                Console.WriteLine(message);

            }
        }
    }
}
