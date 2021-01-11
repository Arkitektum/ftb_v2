using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Altinn2.Adapters.WS.Correspondence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altinn.Distribution
{
    public class Altinn2Distribution : IDistributionAdapter
    {
        private readonly ILogger<Altinn2Distribution> _logger;
        private readonly IPrefillAdapter _prefillAdapter;
        private readonly ICorrespondenceAdapter _correspondenceAdapter;
        private readonly IOptions<CorrespondenceConnectionSettings> _connectionOptions;

        public Altinn2Distribution(ILogger<Altinn2Distribution> logger, 
                                   IPrefillAdapter prefillAdapter,
                                   ICorrespondenceAdapter correspondenceAdapter,
                                   IOptions<CorrespondenceConnectionSettings> connectionOptions)
        {
            _logger = logger;
            _prefillAdapter = prefillAdapter;
            _correspondenceAdapter = correspondenceAdapter;
            _connectionOptions = connectionOptions;
        }

        public async Task<IEnumerable<DistributionResult>> SendDistribution(AltinnDistributionMessage altinnMessage)
        {
            var results = new List<DistributionResult>();
            
            //Send prefill
            var prefillResults = await _prefillAdapter.SendPrefill(altinnMessage);
            results.AddRange(prefillResults);

            if (prefillResults.Where(p => p.Step == Common.DistributionStep.Sent).FirstOrDefault() != null)
            {
                //Send correspondence
                //prefillResult.PrefillReferenceId

                if (altinnMessage.NotificationMessage?.ReplyLink?.UrlTitle != string.Empty)
                {
                    var prefillSentResult = prefillResults.Where(o => o is PrefillSentResult).FirstOrDefault() as PrefillSentResult;
                    //altinnMessage.ReplyLink.Url = "{{placeholder:altinnServer}}/Pages/ServiceEngine/Dispatcher/Dispatcher.aspx?ReporteeElementID={{placeholder:prefillFormId}}";
                    var baseAddress = _connectionOptions.Value.BaseAddress;
                    altinnMessage.NotificationMessage.ReplyLink.Url = $"{baseAddress}/Pages/ServiceEngine/Dispatcher/Dispatcher.aspx?ReporteeElementID={prefillSentResult?.PrefillReferenceId}";
                }

                var correspondenceResults = await _correspondenceAdapter.SendMessageAsync(altinnMessage.NotificationMessage, altinnMessage.DistributionFormReferenceId.ToString());

                results.AddRange(correspondenceResults);
            }

            return results;
        }
    }
}
