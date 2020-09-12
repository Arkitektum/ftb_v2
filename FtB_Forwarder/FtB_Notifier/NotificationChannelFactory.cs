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
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new NotificationDefaultPrepareStrategy(form));
        }

        public override IStrategy CreateSendStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new NotificationDefaultSendStrategy(form));
        }

        public override IStrategy CreateReportStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new NotificationDefaultReportStrategy(form));
        }
    }
}
