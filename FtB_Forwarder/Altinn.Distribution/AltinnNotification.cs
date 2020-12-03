using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Distribution
{
    public class AltinnNotification : INotificationAdapter
    {
        private readonly ILogger<AltinnNotification> _logger;
        private readonly ICorrespondenceAdapter _correspondenceAdapter;

        public AltinnNotification(ILogger<AltinnNotification> logger, ICorrespondenceAdapter correspondenceAdapter)
        {
            _logger = logger;
            _correspondenceAdapter = correspondenceAdapter;
        }

        public IEnumerable<DistributionResult> SendNotification(AltinnMessageBase altinnMessage)
        {
            var distributionResult = _correspondenceAdapter.SendMessage(altinnMessage);
            return distributionResult.Result;
        }
    }
}
