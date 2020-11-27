using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;

namespace Bari.Api.Messages
{
    public class HelloWorldMessageInput
    {
        public HelloWorldMessageInput(string text, string exchangeName, string queueName)
        {
            this.id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            Origin = "hello-world-service";
            Text = text;
            ExchangeName = exchangeName;
            QueueName = queueName;
        }

        public Guid id { get; }
        public DateTime TimeStamp { get; }
        public string Origin{ get; }
        public string Text { get; }
        public string ExchangeName{ get; }
        public string QueueName { get; }
    }
}
