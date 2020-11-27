using EasyNetQ;
using EasyNetQ.Topology;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Bari.Api.AMQP.Pub
{
    public class PubMessageManager : IPubMessageManager
    {
        public async Task SendMsgAsync(PubArgs pushMsg, IBus bus)
        {
            //One to one push

            IExchange exchange = null;
            var message = new Message<object>(pushMsg.Message);

            exchange = bus.Advanced.ExchangeDeclare(pushMsg.ExchangeName, ExchangeType.Topic);

            await bus.Advanced.PublishAsync(exchange, pushMsg.QueueName, false, message)
             .ContinueWith(task =>
              {
                  if (!task.IsCompleted && task.IsFaulted)//Message delivery failed
                  {
                      var queue = bus.Advanced.QueueDeclare($"{pushMsg.QueueName}.dlq");
                      bus.Advanced.Bind(exchange, queue, $"{pushMsg.QueueName}.dlq");
                      bus.Advanced.Publish(exchange, pushMsg.QueueName, false, message);
                  }
              });
        }

        public void SendMsg(PubArgs pushMsg, IBus bus)
        {
            IExchange exchange = bus.Advanced.ExchangeDeclare(pushMsg.ExchangeName, ExchangeType.Topic);

            var message = new Message<object>(pushMsg.Message);

            try
            {
                var queue = bus.Advanced.QueueDeclare(pushMsg.QueueName);
                bus.Advanced.Bind(exchange, queue, pushMsg.QueueName);
                bus.Advanced.Publish(exchange, pushMsg.QueueName, false, message);

            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }
    }
}