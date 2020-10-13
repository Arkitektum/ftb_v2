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

        public void SendNotification(AltinnMessageBase altinnMessage)
        {
            _correspondenceAdapter.SendMessage(altinnMessage);
        }
    }
}
