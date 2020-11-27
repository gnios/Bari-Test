using Bari.Api.EventBus;
using Microsoft.AspNetCore.Mvc;

namespace Bari.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWordController : ControllerBase
    {
        private IHelloWorldEventBusManager _publisher;

        public HelloWordController(IHelloWorldEventBusManager publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public void Post()
        {
            _publisher.Publish("Hello World");
        }
    }
}