using Bari.Api.AMQP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Bari.Api.SignalR
{
    public class QueuetHub : Hub<IQueueClient>
    {
        public async Task SendMessage(HelloWorldMessageOutPut message)
        {
            await Clients.All.SendMessageOutPut(message);
        }
    }
}