using Microsoft.AspNetCore.Mvc;
using NotificationService.Repositories.Interfaces;

namespace NotificationService.Controllers;

[Route("api/[controller]")]
[ApiController]

public class NotificationController : ControllerBase
{
    private readonly INotificationRepository _repo;


    public NotificationController(INotificationRepository repo)
    {
        _repo = repo;
    }
    
    // POST
    
    // GET
    
}