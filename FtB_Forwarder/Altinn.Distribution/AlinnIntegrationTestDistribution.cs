using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Altinn.Distribution
{
    public class AlinnIntegrationTestDistribution : IDistributionAdapter
    {
        public async Task<IEnumerable<DistributionResult>> SendDistribution(AltinnDistributionMessage altinnMessage)
        {
            var results = new List<DistributionResult>();
            
            //Prefill
            results.Add(new DistributionResult(DistributionComponent.Prefill) { Message = "Ok", Step = DistributionStep.Sent });
            var prefillSleepMs = new Random(5).Next(100, 600);
            await Task.Delay(prefillSleepMs);

            //Thread.Sleep(prefillSleepMs);

            //Correspondence
            results.Add(new DistributionResult(DistributionComponent.Correspondence) { Message = "Ok", Step = DistributionStep.Sent });
            var correspondenceSleepMs = new Random(5).Next(100, 600);
            //Thread.Sleep(correspondenceSleepMs);
            await Task.Delay(prefillSleepMs);

            return results;
        }
    }
}
