using FtB_Common.Factories;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Forms;
using FtB_NotificationForwarding;
using FtB_NotificationForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_InitiateForwarding
{
    public static class FormatIdToChannelMapper
    {
        public static AbstractChannelFactory GetChannelFactory(string formatID)
        {
            //Les dette fra konfigurasjon i stedet fra casen under
            switch (formatID)
            {
                case "6325":
                    return new DistributionChannelFactory();
                case "6173":
                    return new NotificationChannelFactory();
                case "12345":
                    //Channel = new NokLittNyttDistributionChannelFactory();
                    return null;
                default:
                    return null;
            }
        }
    }
}
