using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using APIConsumer;
using RealState.Models;

namespace RealState.ReadStack
{
  public class AgentStatisticsQueries: IAgentStatisticsQueries
    {
        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();
        private readonly RealStateStatisticsClient _statisticsClient;
        private readonly int topNumberNeeded = 10;
        public AgentStatisticsQueries(RealStateStatisticsClient statisticsClient)
        {
            _statisticsClient = statisticsClient??throw new ArgumentNullException(nameof(statisticsClient));
        }

        public async Task<IEnumerable<MakelaarObject>> GetTopAgentsByObjects(bool withGarden)
        {
            var objectList = await _statisticsClient.GetRealStateObjects(withGarden,_cancellationTokenSource.Token);

            return objectList.Objects.GroupBy(x => x.MakelaarId)
                .OrderByDescending(g => g.Count()).Take(topNumberNeeded)
                .ToList()
                .Select(agentGroup => new MakelaarObject {MakelaarId = agentGroup.Key, ObjectsNumber = agentGroup.Count()})
                .ToList();


        }
    }
}
