using FtB_CommonModel.Factories;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Forms;
using FtB_NotificationForwarding;
using FtB_NotificationForwarding.Forms;
using FtB_ShipmentForwarding;
using FtB_ShipmentForwarding.Forms;
using System;

namespace FtB_InitiateForwarding
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Oppstart");

            if (args != null || args.Length == 2)
            {
                string formatID = args[0];
                string processStep = args[1];
                Console.WriteLine("formatID: " + formatID + ", processStep: " + processStep);

                FormFormatIdMapper mapper = new FormFormatIdMapper(formatID);
                Forwarder channelForwarder = new Forwarder(mapper.ProcessChannel, mapper.Form);
            
                switch (processStep)
                {
                    case "P":
                        channelForwarder.PrepareFormForForwarding();
                        break;
                    case "E":
                        channelForwarder.ExecuteForwarding();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Bruk: arg1 (formatId), arg2 (prosess-steg (P/E/R)");
            }
        }
    }
}
