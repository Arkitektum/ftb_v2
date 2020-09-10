using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentChannelFactory : AbstractProcessStepFactory
    {
        public override PrepareForwarding CreatePrepareForwarding(Form form)
        {
            return new ShipmentPrepareForwarder(form);
        }

        public override ExceuteForwarding CreateExceuteForwarding(Form form)
        {
            return new ShipmentExecuteForwarder();
        }

        public override ReportForwarding CreateReportForwarding()
        {
            return new ShipmentReportForwarder();
        }
    }
}
