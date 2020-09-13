using FtB_Common;
using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_NotificationForwarding.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding
{
    public class NotificationChannelFactory : AbstractChannelFactory
    {
        public override IStrategy CreatePrepareStrategy(IForm form)
        {
            return (form.GetCustomizedPrepareStrategy() != null
                ? form.GetCustomizedPrepareStrategy() : new DefaultNotificationPrepareStrategy(form));
        }

        public override IStrategy CreateSendStrategy(IForm form)
        {
            return (form.GetCustomizedSendStrategy() != null
                ? form.GetCustomizedSendStrategy() : new DefaultNotificationSendStrategy(form));
        }

        public override IStrategy CreateReportStrategy(IForm form)
        {
            return (form.GetCustomizedReportStrategy() != null
                ? form.GetCustomizedReportStrategy() : new DefaultNotificationReportStrategy(form));
        }
    }
}
