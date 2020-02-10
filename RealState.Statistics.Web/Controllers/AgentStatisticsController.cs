using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealState.ReadStack;

namespace RealState.Statistics.Web.Controllers
{
    [Route("AgentStatistics")]
    [Controller]
    public class AgentStatisticsController : Controller
    {
        private readonly IAgentStatisticsQueries _agentStatisticsQueries;

        public AgentStatisticsController(IAgentStatisticsQueries agentStatisticsQueries)
        {
            _agentStatisticsQueries = agentStatisticsQueries??throw new ArgumentNullException(nameof(agentStatisticsQueries));
        }
        //// GET: AgentStatistics
        //public async Task<IActionResult> TopAgents()
        //{
        //    var topAgentsByObjects =await _agentStatisticsQueries.GetTopAgentsByObjects(true);
        //    return PartialView("TopAgents",topAgentsByObjects);
        //}
        [HttpGet("{isGardenChecked}")]
        public async Task<IActionResult> TopAgents(bool isGardenChecked)
        {
            var topAgentsByObjects = await _agentStatisticsQueries.GetTopAgentsByObjects(isGardenChecked);
            return PartialView("TopAgents", topAgentsByObjects);
        }





    }
}