using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Altinn.Distribution
{
    public class Altinn2Distribution : IDistributionAdapter
    {
        private readonly ILogger<Altinn2Distribution> _logger;
        private readonly IPrefillAdapter _prefillAdapter;
        private readonly ICorrespondenceAdapter _correspondenceAdapter;

        public Altinn2Distribution(ILogger<Altinn2Distribution> logger, IPrefillAdapter prefillAdapter, ICorrespondenceAdapter correspondenceAdapter)
        {
            _logger = logger;
            _prefillAdapter = prefillAdapter;
            _correspondenceAdapter = correspondenceAdapter;
        }

        public IEnumerable<DistributionResult> SendDistribution(AltinnDistributionMessage altinnMessage)
        {
            var results = new List<DistributionResult>();
            
            //Send prefill
            var prefillResults = _prefillAdapter.SendPrefill(altinnMessage);
            results.AddRange(prefillResults);

            if (prefillResults.Where(p => p.Step == Common.DistriutionStep.Sent).FirstOrDefault() != null)
            {
                //Send correspondence
                //prefillResult.PrefillReferenceId

                if (altinnMessage.NotificationMessage?.ReplyLink?.UrlTitle != string.Empty)
                {
                    var prefillSentResult = prefillResults.Where(o => o is PrefillSentResult).FirstOrDefault() as PrefillSentResult;
                    //altinnMessage.ReplyLink.Url = "{{placeholder:altinnServer}}/Pages/ServiceEngine/Dispatcher/Dispatcher.aspx?ReporteeElementID={{placeholder:prefillFormId}}";
                    altinnMessage.NotificationMessage.ReplyLink.Url = $"https://tt02.altinn.no/Pages/ServiceEngine/Dispatcher/Dispatcher.aspx?ReporteeElementID={prefillSentResult?.PrefillReferenceId}";
                }

                var correspondenceResults = _correspondenceAdapter.SendMessage(altinnMessage.NotificationMessage, altinnMessage.DistributionFormReferenceId.ToString());

                results.AddRange(correspondenceResults);
            }

            return results;
        }
    }
}
