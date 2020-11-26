using Bari.Api.AMQP;
using Microsoft.AspNetCore.Mvc;

namespace Bari.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWord : ControllerBase
    {
        public IPublisher Publisher { get; }

        public HelloWord(IPublisher publisher)
        {
            Publisher = publisher;
        }

        [HttpPost]
        public void Post()
        {
            Publisher.Init();
        }
    }
}