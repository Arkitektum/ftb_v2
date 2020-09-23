using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;

namespace FtB_DistributionDataModels.Forms
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824")]
    public class NaboVarselPlanForm : DistributionFormBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public NaboVarselPlanForm(IFormDataRepo dataRepo) : base(dataRepo)
        {
            Name = "Distribusjon av nabovarsel for plan";
            SchemaFile = "nabovarselPlan.xsd";            
        }

        
        private void GetReceivers()
        {
            foreach (var beroertPart in _dataForm.beroerteParter)
            {
                ReceiverIdentifers.Add(beroertPart.navn);
            }
        }
        public string GetFormatId()
        {            
            return _dataForm.dataFormatId;
        }

        public override void OptionalMethod()
        {
            base.OptionalMethod();
            Console.WriteLine("Valgfri metode implementert for skjema NABOVARSELPLAN");
        }

        public override void InitiateForm()
        {
            GetReceivers();
        }
    }
}
