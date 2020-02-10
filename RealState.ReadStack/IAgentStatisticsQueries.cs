using System.Collections.Generic;
using System.Threading.Tasks;
using RealState.Models;

namespace RealState.ReadStack
{
    public interface IAgentStatisticsQueries
    {
        Task<IEnumerable<MakelaarObject>> GetTopAgentsByObjects(bool withGarden);
    }
}