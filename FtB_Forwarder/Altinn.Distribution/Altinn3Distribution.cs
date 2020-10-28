using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Altinn.Distribution
{
    public class Altinn3Distribution : IDistributionAdapter
    {
        private readonly ILogger<Altinn3Distribution> _logger;
        private readonly IPrefillAdapter _prefillAdapter;
        private readonly ICorrespondenceAdapter _correspondenceAdapter;

        public Altinn3Distribution(ILogger<Altinn3Distribution> logger, IPrefillAdapter prefillAdapter, ICorrespondenceAdapter correspondenceAdapter)
        {
            _logger = logger;
            _prefillAdapter = prefillAdapter;
            _correspondenceAdapter = correspondenceAdapter;
        }

        IEnumerable<DistributionResult> IDistributionAdapter.SendDistribution(AltinnDistributionMessage altinnMessage)
        {
            var results = new List<DistributionResult>();
            //Send prefill
            var prefillResult = _prefillAdapter.SendPrefill(altinnMessage);

            if (prefillResult.Where(r => r.Step == DistriutionStep.Sent).Any())
            {
                //Send correspondence
                //prefillResult.PrefillReferenceId
                //Transform body!!!!! 
                //, prefillResult.PrefillReferenceId
                var correspondenceResults = _correspondenceAdapter.SendMessage(altinnMessage.NotificationMessage);

                results.AddRange(correspondenceResults); 
            }           

            return results;
        }        
    }
}
