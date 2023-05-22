using back_risk_register.Models;
using back_risk_register.DataBase;
using back_risk_register.Interfaces;
using back_risk_register.DataBase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace back_risk_register.Services
{

    public class RiskService : IRisk
    {
    /*    private readonly DataTaskRegister _dataTaskRegister;

        public RiskService(DataTaskRegister dataTaskRegister)
        {
            _dataTaskRegister = dataTaskRegister;
        }*/
        public RiskService() { }
        DataTaskRegister _dataTaskRegister = new DataTaskRegister();
        public Task<int> GetLastRiskRegisterId()
        {
            return _dataTaskRegister.GetLastRiskRegisterId();
        }

        public Task CreateRisks([FromBody] List<Risk> riskList)
        {
            return _dataTaskRegister.CreateRisks(riskList);
        }

        public Task UpdateRisks(List<Risk> riskList, HttpResponse res)
        {
            return _dataTaskRegister.UpdateRisks(riskList, res);
        }

        public Task DeleteRisks(List<dynamic> idList)
        {
            return _dataTaskRegister.DeleteRisks(idList);
        }

        public Task<List<Risk>> GetRisksByIdPlan(int idPlan)
        {
            return _dataTaskRegister.GetRisksByIdPlan(idPlan);
        }
    }
}
