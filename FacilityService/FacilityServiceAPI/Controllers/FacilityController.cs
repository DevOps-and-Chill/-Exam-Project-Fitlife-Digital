using Microsoft.AspNetCore.Mvc;

namespace FacilityServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilityController : ControllerBase
    {
       

        private readonly ILogger<FacilityController> _logger;

        public FacilityController(ILogger<FacilityController> logger)
        {
            _logger = logger;
        }

      
    }
}
