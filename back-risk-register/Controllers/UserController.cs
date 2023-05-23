using back_risk_register.Models;
using back_risk_register.Services;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _service;
        public UserController(IUser service) {
            _service = service;
        }

        [HttpPost]
        public IActionResult create(User data)
        {
            return _service.create(data);
        }
    }
}
