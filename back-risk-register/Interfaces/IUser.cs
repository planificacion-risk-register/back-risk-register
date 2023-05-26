using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Services
{
    public interface IUser
    {
        public IActionResult create([FromBody] User user);
        public IActionResult update([FromBody] User user);
        public IActionResult getUsers();
        public IActionResult getUsersByEmail([FromBody] string email);
    }
}
