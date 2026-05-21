
using Microsoft.AspNetCore.Mvc;
using ClassServiceAPI.Messaging;

namespace ClassServiceAPI.Controllers;

[Route("api/[controller]")]
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
            ClassId   = Guid.NewGuid(),
            Title     = "Test Yoga Class",
            TimeStart = DateTime.Now,
            TimeEnd   = DateTime.Now.AddHours(1),
            MemberIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        await _publisher.PublishAsync(classMessage, "class.cancelled");
        return Ok("Besked sendt!");
    }
}