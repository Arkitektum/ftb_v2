using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding
{
    public class DistributionChannelFactory : AbstractChannelFactory
    {
        public override PrepareStrategyBase CreatePrepareBase(FormBase form)
        {
            return new DistributionPrepareStrategy(form);
        }
        //public override PrepareForwarding CreateAnnslessPrepareForwarding(Form form)
        //{
        //    return new AnnslessDistributionPrepareForwarder(form);
        //}


        public override SendStrategyBase CreateSendBase(FormBase form)
        {
            return new DistributionSendStrategy(form);
        }

        public override ReportStrategyBase CreateReportBase()
        {
            return new DistributionReportStrategy();
        }
    }
}
