using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Services
{
    public interface IUser
    {
        public List<User> create([FromBody] User user);
        public List<User> update([FromBody] User user);
        public List<User> getUsers();
        public List<User> getUsersByEmail(String email);
    }
}
