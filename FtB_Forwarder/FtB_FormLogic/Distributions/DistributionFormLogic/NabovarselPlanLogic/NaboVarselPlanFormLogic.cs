using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    //[FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824")]
    //public class NaboVarselPlanFormLogic : DistributionFormLogicBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType, no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    //{
    //    public NaboVarselPlanFormLogic(IFormDataRepo dataRepo, IPrefillDataProvider<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType> prefillDataProvider) : base(dataRepo, prefillDataProvider)
    //    {
    //        Name = "Distribusjon av nabovarsel for plan";
    //        SchemaFile = "nabovarselPlan.xsd";
    //        base.Mapper = new NabovarselPlanPrefillMapper();
    //    }

    //    private List<Receiver> receivers;
    //    public override List<Receiver> Receivers
    //    {
    //        get
    //        {
    //            if (receivers == null)
    //            {
    //                receivers = new List<Receiver>();
    //                foreach (var beroertPart in DataForm.beroerteParter)
    //                {
    //                    Enum.TryParse(beroertPart.partstype.kodeverdi, out ReceiverType receiverType);
    //                    string id;
    //                    if (receiverType.Equals(ReceiverType.Privatperson))
    //                    {
    //                        id = beroertPart.foedselsnummer;
    //                    }
    //                    else
    //                    {
    //                        id = beroertPart.organisasjonsnummer;
    //                    }
    //                    receivers.Add(new Receiver() { Type = receiverType, Id = id });
    //                }
    //            };

    //            return receivers;
    //        }
    //    }

    //    public override void InitiateForm()
    //    {
    //    }
    //}
}
