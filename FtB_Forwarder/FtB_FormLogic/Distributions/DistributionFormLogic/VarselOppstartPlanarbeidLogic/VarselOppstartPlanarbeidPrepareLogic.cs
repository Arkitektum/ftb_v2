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
        public VarselOppstartPlanarbeidPrepareLogic(IFormDataRepo repo, 
                                                    ITableStorage tableStorage, 
                                                    ILogger<VarselOppstartPlanarbeidPrepareLogic> log, 
                                                    DbUnitOfWork dbUnitOfWork, 
                                                    IDecryptionFactory decryptionFactory) : 
            base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory)
        { }


        public override void SetSender()
        {
            Enum.TryParse(FormData.forslagsstiller.partstype.kodeverdi, out ActorType receiverType);
            string id;
            if (receiverType.Equals(ActorType.Privatperson))
            {
                id = FormData.forslagsstiller.foedselsnummer;
            }
            else
            {
                id = FormData.forslagsstiller.organisasjonsnummer;
            }
        
            Sender = new Actor() { Id = id, Type = receiverType };
        }
        public override void SetReceivers()
        {
            var receivers = new List<Actor>();

            foreach (var beroertPart in this.FormData.beroerteParter)
            {
                Enum.TryParse(beroertPart.partstype.kodeverdi, out ActorType receiverType);
                string id;
                if (receiverType.Equals(ActorType.Privatperson))
                {
                    id = beroertPart.foedselsnummer;
                }
                else
                {
                    id = beroertPart.organisasjonsnummer;
                }
                receivers.Add(new Actor() { Type = receiverType, Id = id });
            }

            base.SetReceivers(receivers);
        }
    }
}
