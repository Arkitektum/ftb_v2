using FtB_Common;
using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_DistributionForwarding.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding
{
    public class DistributionChannelFactory : ChannelFactoryBase
    {
        public override IStrategy CreatePrepareStrategy(IForm form)
        {
            return (form.GetCustomizedPrepareStrategy() != null 
                ? form.GetCustomizedPrepareStrategy() : new DefaultDistributionPrepareStrategy(form));
        }

        public override IStrategy CreateSendStrategy(IForm form)
        {
            return (form.GetCustomizedSendStrategy() != null
                ? form.GetCustomizedSendStrategy() : new DefaultDistributionSendStrategy(form));
        }

        public override IStrategy CreateReportStrategy(IForm form)
        {
            return (form.GetCustomizedReportStrategy() != null
                ? form.GetCustomizedReportStrategy() : new DefaultDistributionReportStrategy(form));
        }
    }
}
