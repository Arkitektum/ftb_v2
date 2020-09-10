using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Forms;
using FtB_NotificationForwarding;
using FtB_NotificationForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_InitiateForwarding
{
    public class FormFormatIdMapper
    {
        public Form Form { get; private set; }
        public AbstractProcessStepFactory ProcessChannel { get; private set; }

        public FormFormatIdMapper(string formatID)
        {
            if (formatID.Equals("6325"))
            {
                Form = new NaboVarselPlanForm();
                ProcessChannel = new DistributionChannelFactory();

            }
            else if (formatID.Equals("6173"))
            {
                Form = new SvarPaaNabovarselForm();
                ProcessChannel = new NotificationChannelFactory();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public FormFormatIdMapper Get()
        {
            return this;
        }

    }
}
