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
            var prefillSleepMs = new Random(5).Next(100, 600);
            await Task.Delay(prefillSleepMs);

            var random = new Random();

            var randomNumber = random.Next(0, 100);

            if (randomNumber < 80)
            {
                results.Add(new DistributionResult(DistributionComponent.Prefill) { Message = "Ok", Step = DistriutionStep.Sent });
                //Correspondence
                results.Add(new DistributionResult(DistributionComponent.Correspondence) { Message = "Ok", Step = DistriutionStep.Sent });
                var correspondenceSleepMs = new Random(5).Next(100, 600);
                await Task.Delay(correspondenceSleepMs);
            }
            else
            {
                if (randomNumber >= 80 && randomNumber < 90)
                    results.Add(new DistributionResult(DistributionComponent.Prefill) { Message = "Reserved", Step = DistriutionStep.ReservedReportee });
                else if (randomNumber >= 90 && randomNumber < 95)
                    results.Add(new DistributionResult(DistributionComponent.Prefill) { Message = "Unable to reach", Step = DistriutionStep.UnableToReachReceiver });
                else
                    results.Add(new DistributionResult(DistributionComponent.Prefill) { Message = "Error", Step = DistriutionStep.UnkownErrorOccurred });
            }

            return results;
        }
    }
}
