using Azure;
using back_risk_register.Models;
using back_risk_register.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskController : ControllerBase
    { 
        RiskService _riskService = new RiskService();
    /*  private readonly RiskService _riskService;

      public RiskController(RiskService riskService)
      {
          _riskService = riskService;
      }*/

    [HttpGet("lastId")]
    public async Task<ActionResult<int>> GetLastRiskRegisterId()
    {
        int lastId = await _riskService.GetLastRiskRegisterId();
        return Ok(lastId);
    }

    [HttpPost("create")]
    [Authorize]
        public async Task<IActionResult> CreateRisks([FromBody] List<Risk> riskList)
    {
        await _riskService.CreateRisks(riskList);
        return Ok();
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateRisks([FromBody] List<Risk> riskList)
    {
        await _riskService.UpdateRisks(riskList, Response);
        return Ok();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteRisks([FromBody] List<int> idList)
    {
        await _riskService.DeleteRisks(idList);
        return Ok();
    }

    [HttpGet("byIdPlan/{idPlan}")]
    [Authorize]
        public async Task<ActionResult<List<Risk>>> GetRisksByIdPlan(int idPlan)
    {
        List<Risk> risks = await _riskService.GetRisksByIdPlan(idPlan);
        return Ok(risks);
    }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllRisks(int id)
        {
            await _riskService.DeleteAll(id, Response);
            return Ok();
        }
    }
}