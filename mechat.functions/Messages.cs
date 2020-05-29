using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using mechat.messages;

namespace mechat.functions
{
    public static class Messages
    {
        [FunctionName("Messages")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post")] HttpRequest req,
            [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages, 
            ILogger log)
        {

            var serializedObject = new JsonSerializer()
                .Deserialize(new JsonTextReader(new StreamReader(req.Body)));

            var message =
                JsonConvert
                .DeserializeObject<SimpleTextMessage>(serializedObject.ToString());

            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = "ReceiveMessage",
                Arguments = new[] { message }
            });

        }
    }
}
