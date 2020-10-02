using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APIConsumer;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RealState.Models;

namespace RealState.ReadStack
{
    public class AgentStatisticsQueries : IAgentStatisticsQueries
    {
        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

        private readonly RealStateStatisticsClient _statisticsClient;
        private readonly int topNumberNeeded = 10;
        IDistributedCache _distributedCache;
        public AgentStatisticsQueries(RealStateStatisticsClient statisticsClient,IDistributedCache distributedCache)
        {
            _statisticsClient = statisticsClient ?? throw new ArgumentNullException(nameof(statisticsClient));
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<MakelaarObject>> GetTopAgentsByObjects(bool withGarden)
        {
            List<MakelaarObject> rsObjects =null;
            string cachStr = null;

            //Check if the value in the cache first
            if (withGarden)
            {
                cachStr = _distributedCache.GetString("rs-WithGarden");
            }
            else
            {
                cachStr = _distributedCache.GetString("rs-NoGarden");

            }
            if (!string.IsNullOrEmpty(cachStr))
            {
                rsObjects = JsonConvert.DeserializeObject<List<MakelaarObject>>(cachStr);
            }
            else
            {
                var allObj = await _statisticsClient.GetRealStateObjects(withGarden, _cancellationTokenSource.Token);
                rsObjects = allObj.Objects.GroupBy(x => x.MakelaarId)
             .OrderByDescending(g => g.Count()).Take(topNumberNeeded).ToList().Select(agentGroup => new MakelaarObject { MakelaarId = agentGroup.Key, ObjectsNumber = agentGroup.Count() }).ToList();

                //Setting cache invalidation policy
                DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
                cacheEntryOptions.SetAbsoluteExpiration(new TimeSpan(12, 0, 0));
                if (withGarden)
                {
                    _distributedCache.SetString("rs-WithGarden", JsonConvert.SerializeObject(rsObjects), cacheEntryOptions);
                }
                else
                {
                    _distributedCache.SetString("rs-NoGarden", JsonConvert.SerializeObject(rsObjects), cacheEntryOptions);
                }

            }

            return rsObjects;
            
        }
    }
}