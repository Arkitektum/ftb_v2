using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Prepare)]
    public class VarselOppstartPlanarbeidPrepareLogic : DistributionPrepareLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public VarselOppstartPlanarbeidPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger<VarselOppstartPlanarbeidPrepareLogic> log, DbUnitOfWork dbUnitOfWork, IDecryptionFactory decryptionFactory) : 
            base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory)
        { }

        //private List<Receiver> receivers;
        //protected override List<Receiver> Receivers
        //{
        //    get
        //    {
        //        if (receivers == null)
        //        {
        //            receivers = new List<Receiver>();

        //            foreach (var beroertPart in this.FormData.beroerteParter)
        //            {
        //                Enum.TryParse(beroertPart.partstype.kodeverdi, out ReceiverType receiverType);
        //                string id;
        //                if (receiverType.Equals(ReceiverType.Privatperson))
        //                {
        //                    id = beroertPart.foedselsnummer;
        //                }
        //                else
        //                {
        //                    id = beroertPart.organisasjonsnummer;
        //                }
        //                receivers.Add(new Receiver() { Type = receiverType, Id = id });
        //            }
        //            base.Receivers = receivers;
        //        };

        //        return base.Receivers;
        //    }
        //    set { base.Receivers = value; }
        //}

        public override void SetReceivers()
        {
            var receivers = new List<Receiver>();

            foreach (var beroertPart in this.FormData.beroerteParter)
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

            base.SetReceivers(receivers);
        }
    }
}
