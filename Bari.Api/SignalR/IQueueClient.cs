using System.Threading.Tasks;

namespace Bari.Api.SignalR
{
    public interface IQueueClient
    {
        Task SendMessageOutPut(HelloWorldMessageOutPut message);
    }
}