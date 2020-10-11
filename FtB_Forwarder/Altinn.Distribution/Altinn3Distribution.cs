using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
        
        public IEnumerable<AltinnDistributionResult> SendDistribution(AltinnDistributionMessage altinnMessage)
        {
            var results = new List<AltinnDistributionResult>();
            //Send prefill
            var prefillResult = _prefillAdapter.SendPrefill(altinnMessage);

            if (prefillResult.ResultType == Common.PrefillResultType.Ok)
            {
                results.Add(new AltinnDistributionResult() { Status = AltinnDistributionStatus.PrefillSent });

                //Send correspondence
                //prefillResult.PrefillReferenceId
                _correspondenceAdapter.SendMessage(altinnMessage, prefillResult.PrefillReferenceId);

                results.Add(new AltinnDistributionResult() { Status = AltinnDistributionStatus.MessageSent });
            }
            else
            {
                results.Add(new AltinnDistributionResult() { Status = AltinnDistributionStatus.PrefillFailed });
            }
            

            return results;
        }


    }
}
