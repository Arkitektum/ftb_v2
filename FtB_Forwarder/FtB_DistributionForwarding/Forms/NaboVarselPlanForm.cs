using FtB_CommonModel.Forms;
using FtB_CommonModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Forms
{
    public class NaboVarselPlanForm : DistributionForm, IForm
    {


        private no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType _form;

        public override void ProcessPrepareStep()
        {
            Console.WriteLine("Spesialhåndtering av skjema for NABOVARSELPLAN");
            this.OptionalMethod();
        }
        public override void OptionalMethod()
        {
            base.OptionalMethod();
            Console.WriteLine("Valgfri metode implementert for skjema NABOVARSELPLAN");
        }

        public override void InitiateForm(string formDataAsXml)
        {
            throw new NotImplementedException();
        }
    }
}
