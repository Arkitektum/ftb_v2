using FtB_Common;
using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_DistributionForwarding.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding
{
    public class DistributionChannelFactory : AbstractChannelFactory
    {
        public override IStrategy CreatePrepareStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null 
                ? form.GetPrepareStrategy() : new DistributionDefaultPrepareStrategy(form));
        }

        public override IStrategy CreateSendStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new DistributionDefaultSendStrategy(form));
        }

        public override IStrategy CreateReportStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new DistributionDefaultReportStrategy(form));
        }
    }
}
