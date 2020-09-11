using FtB_CommonModel.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Forms
{
    public class SvarPaaNabovarselForm : FormBase
    {
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
