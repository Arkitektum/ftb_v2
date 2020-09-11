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
        public override PrepareBase CreatePrepareBase(FormBase form)
        {
            return new NotificationPreparer(form);
        }

        public override SendBase CreateSendBase(FormBase form)
        {
            return new NotificationSender(form);
        }

        public override Reportbase CreateReportBase()
        {
            return new NotificationReporter();
        }
    }
}
