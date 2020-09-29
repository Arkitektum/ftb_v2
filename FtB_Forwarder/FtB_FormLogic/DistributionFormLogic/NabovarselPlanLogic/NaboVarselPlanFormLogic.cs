using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824")]
    public class NaboVarselPlanFormLogic : DistributionFormLogicBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public NaboVarselPlanFormLogic(IFormDataRepo dataRepo) : base(dataRepo)
        {
            Name = "Distribusjon av nabovarsel for plan";
            SchemaFile = "nabovarselPlan.xsd";
                        
        }

        private List<Receiver> receivers;
        public override List<Receiver> Receivers
        {
            get
            {
                if (receivers == null)
                {
                    receivers = new List<Receiver>();
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
                        receivers.Add(new Receiver() { Type = receiverType, Id = id });
                    }
                };

                return receivers;
            }
        }

        //public void GetReceivers()
        //{
        //    foreach (var beroertPart in DataForm.beroerteParter)
        //    {
        //        Enum.TryParse(beroertPart.partstype.kodeverdi, out ReceiverType receiverType);
        //        string id;
        //        if (receiverType.Equals(ReceiverType.Privatperson))
        //        {
        //            id = beroertPart.foedselsnummer;
        //        }
        //        else
        //        {
        //            id = beroertPart.organisasjonsnummer;
        //        }
        //        Receivers.Add(new Receiver() { Type = receiverType, Id = id });
        //    }
        //}
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
            //GetReceivers();
        }

        public override void ProcessSendStep()
        {
            //Should be injected???
            var mapper = new NabovarselPlanPrefillMapper();
            //TODO: Fikk feil ved kjøring: kommentert ut
            //var result = mapper.Map(this.DataForm, "naboidentifikator");

            // Hått gjer me med resultatet tru?

            

        }
    }
}
