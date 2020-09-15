using FtB_Common.Interfaces;
using FtB_Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Forms
{
    //public class NaboVarselPlanForm<T> : DistributionFormBase<T>, IForm
    public class NaboVarselPlanForm : DistributionFormBase, IForm
    {
        private no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType _form;

        public NaboVarselPlanForm() : base()
        {
            Name = "Distribusjon av nabovarsel for plan";
            SchemaFile = "nabovarselPlan.xsd";
        }

        public string GetFormatId()
        {
            return _form.dataFormatId;
        }
        private void Data()
        {
            //_form = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>(formDataAsXml);
        }


        public override void OptionalMethod()
        {
            base.OptionalMethod();
            Console.WriteLine("Valgfri metode implementert for skjema NABOVARSELPLAN");
        }

        public override void InitiateForm(string formDataAsXml)
        {

            _form = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>(formDataAsXml);
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
