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

            if (args != null || args.Length == 1)
            {
                string archiveReference = args[0];
                new Execute(archiveReference);
            }
            else
            {
                Console.WriteLine("Bruk: arg1 (formatId), arg2 (prosess-steg (P/E/R)");
            }
        }
    }
}
