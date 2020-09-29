using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealState.ReadStack;

namespace RealState.Statistics.Web.Controllers
{
    [Controller]
    public class AgentStatisticsController : Controller
    {
        private readonly IAgentStatisticsQueries _agentStatisticsQueries;

        public AgentStatisticsController(IAgentStatisticsQueries agentStatisticsQueries)
        {
           
            _agentStatisticsQueries =
                agentStatisticsQueries ?? throw new ArgumentNullException(nameof(agentStatisticsQueries));
        }

        [HttpGet("{isGardenChecked?}")]
        public async Task<IActionResult> TopAgents(bool isGardenChecked = true)
        {
            if(!isGardenChecked)
                throw new ArgumentNullException("test azure alert");
            var topAgentsByObjects = await _agentStatisticsQueries.GetTopAgentsByObjects(isGardenChecked);
            return PartialView("TopAgents", topAgentsByObjects);
        }
    }
}