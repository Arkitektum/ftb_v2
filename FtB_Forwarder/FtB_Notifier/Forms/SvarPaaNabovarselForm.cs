using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Forms
{
    public class SvarPaaNabovarselForm : BaseForm, IForm
    {
        public override IStrategy GetPrepareStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetSendStrategy()
        {
            throw new NotImplementedException();
        }

        public override void InitiateForm(string formDataAsXml)
        {
            throw new NotImplementedException();
        }

        public override void ProcessPrepareStep()
        {
            Console.WriteLine("Spesialhåndtering av skjema for SVARPÅNABOVARSEL");
        }

        public override void ProcessSendStep()
        {
            throw new NotImplementedException();
        }
    }
}
