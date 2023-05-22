using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Interfaces
{
    public interface IRisk
    {
        Task<int> GetLastRiskRegisterId();
        Task CreateRisks([FromBody] List<Risk> riskList);
        Task UpdateRisks(List<Risk> riskList, HttpResponse res);
        Task DeleteRisks(List<dynamic> idList);
        Task<List<Risk>> GetRisksByIdPlan(int idPlan);
    }
}
