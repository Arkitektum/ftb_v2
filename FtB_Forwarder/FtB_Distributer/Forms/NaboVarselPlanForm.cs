using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Forms
{
    public class NaboVarselPlanForm : DistributionBaseForm, IForm
    {
        private no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType _form;

        public override void OptionalMethod()
        {
            base.OptionalMethod();
            Console.WriteLine("Valgfri metode implementert for skjema NABOVARSELPLAN");
        }

        public override void InitiateForm(string formDataAsXml)
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedPrepareStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedSendStrategy()
        {
            throw new NotImplementedException();
        }

        public IStrategy GetCustomizedReportStrategy()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomPrepareStep()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomSendStep()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomReportStep()
        {
            throw new NotImplementedException();
        }
    }
}
