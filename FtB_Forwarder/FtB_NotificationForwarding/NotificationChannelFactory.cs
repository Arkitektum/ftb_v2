using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding
{
    public class NotificationChannelFactory : AbstractChannelFactory
    {
        public override PrepareStrategyBase CreatePrepareBase(FormBase form)
        {
            return new NotificationPrepareStrategy(form);
        }

        public override SendStrategyBase CreateSendBase(FormBase form)
        {
            return new NotificationSendStrategy(form);
        }

        public override ReportStrategyBase CreateReportBase()
        {
            return new NotificationReportStrategy();
        }
    }
}
