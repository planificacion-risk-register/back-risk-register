using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(Login login)
        {
            return Ok("User login");
        }
    }
}
