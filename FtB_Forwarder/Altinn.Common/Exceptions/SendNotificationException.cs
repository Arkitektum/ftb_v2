using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common.Exceptions
{
    public class SendNotificationException : Exception
    {
        public string Text { get; set; }
        public DistributionStep DistriutionStep { get; private set; }
        public SendNotificationException(string text, DistributionStep distriutionStep)
        {
            Text = text;
            DistriutionStep = distriutionStep;
        }
    }
}
