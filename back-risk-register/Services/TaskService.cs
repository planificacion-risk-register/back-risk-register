using back_risk_register.DataBase;
using back_risk_register.Models;
using back_risk_register.Interfaces;
using System.Numerics;

namespace back_risk_register.Services
{
    public class TaskService
    {
        DataTaskRegister _dataTask = new DataTaskRegister();
        public TaskService() { }

        public async Task DeletePlan(int id_plan, HttpResponse _res)
        {
            
            await _dataTask.DeletePlan(id_plan, _res);
        }

        public async Task InsertPlan(TaskRegister plan, HttpResponse _res)
        {
         
            await _dataTask.InsertPlan(plan, _res);
        }

        public async Task UpdatePlan(TaskRegister plan, HttpResponse _res)
        {
       
            await _dataTask.UpdatePlan(plan, _res);
        }

        public async Task<List<TaskRegister>> GetAllPlans()
        {
         
            return await _dataTask.GetAllPlans();
        }
        
        public async Task<TaskRegister> GetPlan(int id_plan)
        {
            return await _dataTask.GetPlan(id_plan);

        }
    }
}
