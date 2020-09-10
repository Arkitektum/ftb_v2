using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding
{
    public class NotificationChannelFactory : AbstractProcessStepFactory
    {
        public override PrepareForwarding CreatePrepareForwarding(Form form)
        {
            return new NotificationPrepareForwarder(form);
        }

        public override ExceuteForwarding CreateExceuteForwarding(Form form)
        {
            return new NotificationExecuteForwarder();
        }

        public override ReportForwarding CreateReportForwarding()
        {
            return new NotificationReportForwarder();
        }
    }
}
