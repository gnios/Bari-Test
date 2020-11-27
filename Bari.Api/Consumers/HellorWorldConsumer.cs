using Bari.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Bari.Api.Consumers
{
//Message from broadcast mode
public class HellorWorldConsumer : IHellorWorldConsumer
{
    private readonly IHubContext<QueuetHub, IQueueClient> _chatHub;

    public HellorWorldConsumer(IHubContext<QueuetHub, IQueueClient> chatHub)
    {
        _chatHub = chatHub;
    }

    public async void Consume(string message)
    {
        await _chatHub.Clients.All.SendMessageOutPut(new HelloWorldMessageOutPut
        {
            Text = message
        });

    }
}
}