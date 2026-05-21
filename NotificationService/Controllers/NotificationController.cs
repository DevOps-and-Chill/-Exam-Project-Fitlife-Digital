using Microsoft.AspNetCore.Mvc;
using NotificationService.Repositories.Interfaces;

namespace NotificationService.Controllers;

[Route("api/[controller]")]
[ApiController]

public class NotificationController : ControllerBase
{
    private readonly INotificationRepository _repo;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(INotificationRepository repo, ILogger<NotificationController> logger)
    {
        _repo = repo;
        _logger = logger;
    }
    
    // POST
    
    // GET
    
}