﻿using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Services
{
    public interface ILogin
    {
        
            public IActionResult login([FromBody] Login login);

            public IActionResult loginGoogle([FromBody] Login login);

            public IActionResult getAll();
       
    }
}
