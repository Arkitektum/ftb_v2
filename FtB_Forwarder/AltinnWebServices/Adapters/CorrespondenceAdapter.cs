using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn2.Adapters.Adapters
{
    public class CorrespondenceAdapter : ICorrespondenceAdapter
    {
        private readonly ILogger<CorrespondenceAdapter> _logger;

        public CorrespondenceAdapter(ILogger<CorrespondenceAdapter> logger)
        {
            _logger = logger;
        }
        public void SendMessage(AltinnDistributionMessage altinnMessage, string prefillReference)
        {
            _logger.LogDebug("Sending correspondence message!");
        }
    }
}
