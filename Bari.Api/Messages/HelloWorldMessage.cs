using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;

namespace Bari.Api.Messages
{
    public class HelloWorldMessageInput
    {
        public HelloWorldMessageInput(string text, string exchangeNama, string queueName)
        {
            this.id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            Origin = "hello-world-service";
            Text = text;
            ExchangeNama = exchangeNama;
            QueueName = queueName;
        }

        public Guid id { get; }
        public DateTime TimeStamp { get; }
        public string Origin{ get; }
        public string Text { get; }
        public string ExchangeNama{ get; }
        public string QueueName { get; }
    }
}
