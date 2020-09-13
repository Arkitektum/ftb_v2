using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Forms
{
    public class SvarPaaNabovarselForm : BaseForm, IForm
    {
        public override IStrategy GetCustomizedPrepareStrategy()
        {
            throw new NotImplementedException();
        }

        public IStrategy GetCustomizedReportStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedSendStrategy()
        {
            throw new NotImplementedException();
        }

        public override void InitiateForm(string formDataAsXml)
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomPrepareStep()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomReportStep()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomSendStep()
        {
            throw new NotImplementedException();
        }
    }
}
