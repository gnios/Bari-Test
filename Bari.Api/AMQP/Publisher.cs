using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Bari.Api.AMQP
{
    public class Publisher : IPublisher
    {
        public ILogger<Publisher> Logger { get; }

        public Publisher(ILogger<Publisher> logger)
        {
            Logger = logger;
        }

        private string _brokerName = "HelloWorldBroker";

        public async void Init()

        {
            var url = "amqps://sjahetgg:ekV4PyK4NV4ii_Nb5k-_u-elCp9hN0JY@jackal.rmq.cloudamqp.com/sjahetgg";

            var eventName = "HelloWorld";
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(url);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _brokerName, type: "direct");

                //string message = JsonConvert.SerializeObject(@event);

                string message =
                  $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " +
                  $"Message content:";

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: _brokerName,
                    routingKey: eventName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}