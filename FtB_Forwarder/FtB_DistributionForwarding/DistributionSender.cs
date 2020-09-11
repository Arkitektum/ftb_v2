using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding
{
    public class DistributionSender : SendBase, ISender
    {
        public DistributionSender(FormBase form) : base(form)
        {

        }
        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til DISTRIBUTION");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }
    }
}
