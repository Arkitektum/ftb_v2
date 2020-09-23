using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;

namespace FtB_DistributionDataModels.FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824")]
    public class NaboVarselPlanFormLogic : DistributionFormLogicBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public NaboVarselPlanFormLogic(IFormDataRepo dataRepo) : base(dataRepo)
        {
            Name = "Distribusjon av nabovarsel for plan";
            SchemaFile = "nabovarselPlan.xsd";
        }


        public void GetReceivers()
        {
            foreach (var beroertPart in DataForm.beroerteParter)
            {
                Enum.TryParse(beroertPart.partstype.kodeverdi, out ReceiverType receiverType);
                string id;
                if (receiverType.Equals(ReceiverType.Privatperson))
                {
                    id = beroertPart.foedselsnummer;
                }
                else
                {
                    id = beroertPart.organisasjonsnummer;
                }
                Receivers.Add(new Receiver() { Type = receiverType, Id = id });
                }
        }
        public string GetFormatId()
        {
            return DataForm.dataFormatId;
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
