using Bari.Api.AMQP.Pub;
using Bari.Api.AMQP.Sub;
using EasyNetQ;
using System.Threading.Tasks;

namespace Bari.Api.AMQP
{
public interface IRabbitMQManager
{
    IBus CreateEventBus();
    void DisposeBus();
    bool PushMessage(PubArgs pushMsg);
    Task PushMessageAsync(PubArgs pushMsg);
    void Subscribe<TConsumer>(SubArgs args)
        where TConsumer : IMessageConsumer;
}
}