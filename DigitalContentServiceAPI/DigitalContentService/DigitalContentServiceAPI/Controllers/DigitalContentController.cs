using Microsoft.AspNetCore.Mvc;

namespace DigitalContentServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigitalContentController : ControllerBase
    {
       
        private readonly ILogger<DigitalContentController> _logger;

        public DigitalContentController(ILogger<DigitalContentController> logger)
        {
            _logger = logger;
        }

       
    }
}
