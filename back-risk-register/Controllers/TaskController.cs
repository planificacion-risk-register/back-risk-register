using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_risk_register.Models;
using back_risk_register.Services;

namespace back_risk_register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        TaskService _taskService = new TaskService();

        /*  private readonly TaskService _taskService;

          public TaskController()
          {
              _taskService = new TaskService();
          }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            await _taskService.DeletePlan(id, Response);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> InsertPlan([FromBody] TaskRegister plan)
        {
            await _taskService.InsertPlan(plan, Response);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlan([FromBody] TaskRegister plan)
        {
            await _taskService.UpdatePlan(plan, Response);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _taskService.GetAllPlans();
            return Ok(plans);
        }

        [HttpGet("{id}")] // comentaria para quitar los 
        public async Task<IActionResult> GetPlan(int id)
        {
            var plan = await _taskService.GetPlan(id);
            return Ok(plan); 
        }
    }
}

