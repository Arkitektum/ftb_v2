using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;

namespace FtB_DistributionForwarding.Forms
{
    //public class NaboVarselPlanForm<T> : DistributionFormBase<T>, IForm
    [FormDataFormat(DataFormatId = "1234", DataFormatVersion = "6543")]
    public class NaboVarselPlanForm : DistributionFormBase, IForm
    {
        private no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType _form;
        private readonly IFormDataRepo<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType> _dataRepo;

        public NaboVarselPlanForm(IFormDataRepo<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType> dataRepo) : base()
        {
            Name = "Distribusjon av nabovarsel for plan";
            SchemaFile = "nabovarselPlan.xsd";
            _dataRepo = dataRepo;
        }

        public string GetFormatId()
        {
            return _form.dataFormatId;
        }

        public override void OptionalMethod()
        {
            base.OptionalMethod();
            Console.WriteLine("Valgfri metode implementert for skjema NABOVARSELPLAN");
        }

        public override void InitiateForm(string archiveReference)
        {
            _form = _dataRepo.GetFormData(archiveReference);
            //_form = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>(formDataAsXml);
        }

        public override IStrategy GetCustomizedPrepareStrategy()
        {
            return null;
        }

        public override IStrategy GetCustomizedSendStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedReportStrategy()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomPrepareStep()
        {

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
