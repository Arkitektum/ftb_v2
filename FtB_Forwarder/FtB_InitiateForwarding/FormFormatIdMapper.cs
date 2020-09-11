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
        public FormBase Form { get; private set; }
        public AbstractChannelFactory Channel { get; private set; }

        public FormFormatIdMapper(string formatID)
        {
            if (formatID.Equals("6325"))
            {
                Form = new NaboVarselPlanForm();
                Channel = new DistributionChannelFactory();

            }
            else if (formatID.Equals("12345"))
            {
            //    Form = new NokoLittNyttDistributionForm();
            //    ProcessChannel = new NokLittNyttDistributionChannelFactory();
            }
            else if (formatID.Equals("6173"))
            {
                Form = new SvarPaaNabovarselForm();
                Channel = new NotificationChannelFactory();
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
