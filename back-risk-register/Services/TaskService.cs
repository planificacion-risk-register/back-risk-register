using back_risk_register.DataBase;
using back_risk_register.Models;
using back_risk_register.Interfaces;
using System.Numerics;

namespace back_risk_register.Services
{
    public class TaskService
    {
        DataTaskRegister _service = new DataTaskRegister();
        public TaskService() { }

        public async Task DeletePlan(int id_plan, HttpResponse _res)
        {
            
            await _service.DeletePlan(id_plan, _res);
        }

        public async Task InsertPlan(TaskRegister plan, HttpResponse _res)
        {
         
            await _service.InsertPlan(plan, _res);
        }

        public async Task UpdatePlan(TaskRegister plan, HttpResponse _res)
        {
       
            await _service.UpdatePlan(plan, _res);
        }

        public async Task<List<TaskRegister>> GetAllPlans()
        {
         
            return await _service.GetAllPlans();
        }
    }
}
