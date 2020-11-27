namespace Bari.Api.AMQP.Sub
{
public interface IMessageConsumer
{
    void Consume(string message);
}
}