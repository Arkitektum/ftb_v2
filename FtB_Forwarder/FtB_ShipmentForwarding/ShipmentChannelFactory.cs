using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentChannelFactory : AbstractChannelFactory
    {
        public override PrepareStrategyBase CreatePrepareBase(FormBase form)
        {
            return new ShipmentPrepareStrategy(form);
        }

        public override SendStrategyBase CreateSendBase(FormBase form)
        {
            return new ShipmentSendStrategy(form);
        }

        public override ReportStrategyBase CreateReportBase()
        {
            return new ShipmentReportStrategy();
        }
    }
}
