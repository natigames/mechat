using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mechat.core.EventHandlers;
using mechat.core.Models;
using mechat.messages;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace mechat.core.Services
{
    public class ChatService : IChatService
    {
        public ChatService()
        {
        }

        public bool isConnected { get; set;  }

        public string ConnectionToken { get; set;}
        private SemaphoreSlim semaphoreSlim =
            new SemaphoreSlim(1, 1);
        private HttpClient httpClient;
        HubConnection hub;

        public event EventHandler<MessageEventArgs> OnReceivedMessage;


        public async Task InitAsync(string userId)
        {
            await semaphoreSlim.WaitAsync();

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }
            var result = await httpClient
                .GetStringAsync($"{Config.NegotiateEndpoint}/{userId}");
            var info = JsonConvert.DeserializeObject<ConnectionInfo>(result);
            var connectionBuilder =
                new HubConnectionBuilder();

            connectionBuilder.WithUrl(info.Url, (obj) =>
             {
                 obj.AccessTokenProvider = () => Task.Run(() => info.AccessToken);
             });
            hub = connectionBuilder.Build();
            await hub.StartAsync();

            ConnectionToken = hub.ConnectionId;

            isConnected = true;

            hub.On<object>("ReceiveMessage", (message) =>
            {
                var json = message.ToString();
                var obj = JsonConvert.DeserializeObject<ChatMessage>(json);
                var msg = (ChatMessage)JsonConvert.DeserializeObject(json, obj.TypeInfo);
                OnReceivedMessage?.Invoke(this, new MessageEventArgs(msg));
            });

            semaphoreSlim.Release();
        }

        public async Task DisconnectAsync()
        {
            if (!isConnected)
                return;
            try
            {
                await hub.DisposeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            isConnected = false;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            if (!isConnected)
            {
                throw new InvalidOperationException("Not connected");
            }

            var json =
                JsonConvert.SerializeObject(message);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PostAsync(Config.MessagesEndpoint, content);


        }
    }
}
