using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding
{
    public class DistributionChannelFactory : AbstractProcessStepFactory
    {
        public override PrepareForwarding CreatePrepareForwarding(Form form)
        {
            return new DistributionPrepareForwarder(form);
        }

        public override ExceuteForwarding CreateExceuteForwarding(Form form)
        {
            return new DistributionExecuteForwarder();
        }

        public override ReportForwarding CreateReportForwarding()
        {
            return new DistributionReportForwarder();
        }
    }
}
