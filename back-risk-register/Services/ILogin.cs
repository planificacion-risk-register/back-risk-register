using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Services
{
    public interface ILogin
    {
        public Login login([FromBody] Login login);

        public IActionResult getuser(string username);

    }
}
