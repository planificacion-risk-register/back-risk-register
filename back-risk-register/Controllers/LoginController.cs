using back_risk_register.Models;
using back_risk_register.Services;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ILogin login;

        public LoginController(ILogin service)
        {
            login = service;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult listUsersLogin()
        {
            return login.getAll();
        }

        [HttpPost]
        public IActionResult Login(Login data)
        {
            return login.login(data);
        }

        [HttpPost("google")]
        public IActionResult LoginGoogle(Login data)
        {
            return login.loginGoogle(data);
        }
    }
}
