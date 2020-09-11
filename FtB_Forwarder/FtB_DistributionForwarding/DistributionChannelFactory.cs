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
        public override PrepareBase CreatePrepareBase(FormBase form)
        {
            return new DistributionPreparer(form);
        }
        //public override PrepareForwarding CreateAnnslessPrepareForwarding(Form form)
        //{
        //    return new AnnslessDistributionPrepareForwarder(form);
        //}


        public override SendBase CreateSendBase(FormBase form)
        {
            return new DistributionSender(form);
        }

        public override Reportbase CreateReportBase()
        {
            return new DistributionReporter();
        }
    }
}
