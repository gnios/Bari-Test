using Bari.Api;
using Bari.Api.AMQP;
using Bari.Api.AMQP.Pub;
using Bari.Api.AMQP.Sub;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bari.Api.AMQP
{
public class RabbitMQManager : IRabbitMQManager
{

    private volatile static IBus bus = null;

    private readonly object lockHelper = new object();
    private readonly ILogger<RabbitMQManager> _logger;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMQManager(ILogger<RabbitMQManager> logger, IServiceProvider  serviceProvider)
    {
        this._logger = logger;
        this._serviceProvider = serviceProvider;
    }

    public IBus CreateEventBus()
    {
        var config = "amqps://sjahetgg:ekV4PyK4NV4ii_Nb5k-_u-elCp9hN0JY@jackal.rmq.cloudamqp.com/sjahetgg";
        if (string.IsNullOrEmpty(config))
        {
            throw new Exception("Message address is not configured");
        }

        if (bus == null && !string.IsNullOrEmpty(config))
        {
            lock (lockHelper)
            {
                if (bus == null)
                    bus = RabbitHutch.CreateBus(config);
            }
        }
        return bus;
    }

    public bool PushMessage(PubArgs pushMsg)
    {
        bool messangeSent;
        try
        {
            if (bus == null)
                CreateEventBus();
            new PubMessageManager().SendMsg(pushMsg, bus);
            messangeSent = true;
        }
        catch (Exception)
        {
            IExchange exchange = bus.Advanced.ExchangeDeclare(pushMsg.ExchangeName, ExchangeType.Topic);
            var queueName = $"{pushMsg.QueueName}.dlq";
            var queue = bus.Advanced.QueueDeclare(queueName);
            var message = new Message<object>(pushMsg.Message);
            bus.Advanced.Bind(exchange, queue, queueName);
            bus.Advanced.Publish(exchange, queueName, false, message);

            messangeSent = false;
        }

        return messangeSent;
    }

    public async Task PushMessageAsync(PubArgs pushMsg)
    {
        try
        {
            if (bus == null)
                CreateEventBus();
            await new PubMessageManager().SendMsgAsync(pushMsg, bus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public void Subscribe<TConsumer>(SubArgs args)
        where TConsumer : IMessageConsumer
    {
        if (bus == null)
        {
            CreateEventBus();
        }

        if (string.IsNullOrEmpty(args.ExchangeName))
        {
            return;
        }

        Expression<Action<TConsumer>> methodCall;
        IExchange exchange = null;

        exchange = bus.Advanced.ExchangeDeclare(args.ExchangeName, ExchangeType.Topic);

        IQueue queue;
        if (string.IsNullOrEmpty(args.QueueName))
        {
            queue = bus.Advanced.QueueDeclare();
        }
        else
        {
            queue = bus.Advanced.QueueDeclare(args.QueueName);
        }

        bus.Advanced.Bind(exchange, queue, args.QueueName);
        bus.Advanced.Consume(queue, (body, properties, info) => Task.Factory.StartNew(() =>
        {
            try
            {
                lock (lockHelper)
                {

                    var message = Encoding.UTF8.GetString(body);
                    //Processing the message
                    methodCall = job => job.Consume(message);
                    var service = ServiceLocator.Current.GetInstance<TConsumer>();
                    methodCall.Compile()(service);
                }

            }
            catch (Exception ex)
            {
                var queueName = $"{args.QueueName}.dlq";
                var queue = bus.Advanced.QueueDeclare(queueName);
                bus.Advanced.Bind(exchange, queue, queueName);
                bus.Advanced.Publish(exchange, queueName, false, new MessageProperties(), body);

                _logger.LogError(ex, ex.Message);
            }
        }));
    }

    public void DisposeBus()
    {
        bus?.Dispose();
    }
}
}