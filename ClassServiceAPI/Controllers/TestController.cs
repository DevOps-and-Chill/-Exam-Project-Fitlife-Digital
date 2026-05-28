
using Microsoft.AspNetCore.Mvc;
using ClassServiceAPI.Messaging;

namespace ClassServiceAPI.Controllers;

[Route("")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IMessagePublisher _publisher;

    public TestController(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpPost("test-rabbitmq")]
    public async Task<IActionResult> TestRabbitMq()
    {
        var classMessage = new ClassCancelledMessage
        {
            ClassId   = Guid.NewGuid().ToString(),
            Title     = "Test Yoga Class",
            TimeStart = DateTime.Now,
            TimeEnd   = DateTime.Now.AddHours(1),
            ReceiverIds = new List<string> {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            }
        };

        await _publisher.PublishAsync(classMessage, "class.cancelled");
        return Ok("Besked sendt!");
    }
}