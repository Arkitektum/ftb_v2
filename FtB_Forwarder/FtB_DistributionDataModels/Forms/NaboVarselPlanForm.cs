using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;

namespace FtB_DistributionDataModels.Forms
{
    [FormDataFormat(DataFormatId = "6303", DataFormatVersion = "44820")]
    public class NaboVarselPlanForm : DistributionFormBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        private no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType _form;
        //private readonly IFormDataRepo<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType> _dataRepo;

        public NaboVarselPlanForm(IFormDataRepo dataRepo) : base(dataRepo)
        {
            Name = "Distribusjon av nabovarsel for plan";
            SchemaFile = "nabovarselPlan.xsd";            
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
