using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding
{
    public class DistributionPreparer : PrepareBase, IPrepare
    {
        public DistributionPreparer(FormBase form) : base(form)
        {
            
        }
        public override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for DISTRIBUTION");
        }

        public override void ReadReceiverInformation(string archiveReference)
        {
            Console.WriteLine("Leser mottakerinformasjon for DISTRIBUTION");
        }

        public void TransformSubmittalToForwardingMessage()
        {
            Console.WriteLine("Transformerer innsending til (antall) mottakere for DISTRIBUTION");
        }
    }
}
