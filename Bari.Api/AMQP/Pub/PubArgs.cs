namespace Bari.Api.AMQP.Pub
{
public class PubArgs
{
    public object Message { get; set; }

    public string ExchangeName { get; set; }

    public string QueueName { get; set; }
}
}