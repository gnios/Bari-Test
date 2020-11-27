using Bari.Api.AMQP;
using Bari.Api.AMQP.Pub;
using Bari.Api.AMQP.Sub;
using Bari.Api.Consumers;
using Bari.Api.Messages;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using System;

namespace Bari.Api.EventBus
{
    public class HelloWorldEventBusManager : IHelloWorldEventBusManager
    {
        private const string EXCHANGE_NAME = "Gnios.Topic";
        private const string QUEUE_NAME = "Gnios.HelloWorld";
        private readonly IRabbitMQManager _rabbitMQManager;

        private ILogger<HelloWorldEventBusManager> Logger { get; }

        public HelloWorldEventBusManager(ILogger<HelloWorldEventBusManager> logger, IRabbitMQManager rabbitMQManager)
        {
            Logger = logger;
            this._rabbitMQManager = rabbitMQManager;
        }

        public async void Publish(string message)
        {
            var pubMessageArg = new HelloWorldMessageInput(message, EXCHANGE_NAME, QUEUE_NAME);

            var pubMessage = new PubArgs()
            {
                Message = pubMessageArg,
                ExchangeName = EXCHANGE_NAME,
                QueueName = QUEUE_NAME
            };

            await _rabbitMQManager.PushMessageAsync(pubMessage);
        }

        public void Subscribe()
        {
            var subArg = new SubArgs()
            {
                ExchangeName = EXCHANGE_NAME,
                QueueName = QUEUE_NAME
            };

            _rabbitMQManager.Subscribe<IHellorWorldConsumer>(subArg);
        }

        public void PublishEvery(string message, int milliseconds)
        {
            SetInterval(() => this.Publish(message), milliseconds);
        }

        public static System.Timers.Timer SetInterval(Action Act, int Interval)
        {
            System.Timers.Timer tmr = new System.Timers.Timer();
            tmr.Elapsed += (sender, args) => Act();
            tmr.AutoReset = true;
            tmr.Interval = Interval;
            tmr.Start();

            return tmr;
        }
    }
}