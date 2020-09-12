using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Forms
{
    public class ShipmentBaseForm : BaseForm
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
            throw new NotImplementedException();
        }

        public override void ProcessSendStep()
        {
            throw new NotImplementedException();
        }
    }
}
