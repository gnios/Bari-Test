
namespace Bari.Api.EventBus
{
    public interface IHelloWorldEventBusManager
    {
        void Publish(string message);

        void Subscribe();

        void PublishMessageEvery(string message, int milliseconds);
    }
}