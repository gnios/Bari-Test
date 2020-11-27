using EasyNetQ;
using System.Threading.Tasks;

namespace Bari.Api.AMQP.Pub
{
internal interface IPubMessageManager
{
    Task SendMsgAsync(PubArgs pushMsg, IBus bus);

    void SendMsg(PubArgs pushMsg, IBus bus);
}
}