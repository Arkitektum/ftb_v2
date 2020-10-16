using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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

                if (altinnMessage.NotificationMessage?.ReplyLink?.UrlTitle != string.Empty)
                {
                    //altinnMessage.ReplyLink.Url = "{{placeholder:altinnServer}}/Pages/ServiceEngine/Dispatcher/Dispatcher.aspx?ReporteeElementID={{placeholder:prefillFormId}}";
                    altinnMessage.NotificationMessage.ReplyLink.Url = $"https://tt02.altinn.no/Pages/ServiceEngine/Dispatcher/Dispatcher.aspx?ReporteeElementID={prefillResult.PrefillReferenceId}";
                }

                _correspondenceAdapter.SendMessage(altinnMessage.NotificationMessage, altinnMessage.DistributionFormReferenceId);

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
