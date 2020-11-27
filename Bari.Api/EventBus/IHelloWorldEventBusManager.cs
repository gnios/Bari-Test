
namespace Bari.Api.EventBus
{
    public interface IHelloWorldEventBusManager
    {
        void Publish(string message);

        void Subscribe();

        void PublishEvery(string message, int milliseconds);
    }
}