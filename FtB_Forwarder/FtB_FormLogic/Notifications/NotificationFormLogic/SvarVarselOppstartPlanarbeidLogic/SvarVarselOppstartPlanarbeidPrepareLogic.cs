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
    [FormDataFormat(DataFormatId = "6326", DataFormatVersion = "44843", ProcessingContext = FormLogicProcessingContext.Prepare)]
    public class SvarVarselOppstartPlanarbeidPrepareLogic : NotificationPrepareLogic<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public SvarVarselOppstartPlanarbeidPrepareLogic(IFormDataRepo repo,
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

            Enum.TryParse(this.FormData.beroertPart.partstype.kodeverdi, out ActorType receiverType);
            string id;
            if (receiverType.Equals(ActorType.Privatperson))
            {
                id = this.FormData.beroertPart.foedselsnummer;
            }
            else
            {
                id = this.FormData.beroertPart.organisasjonsnummer;
            }
            receivers.Add(new Actor() { Type = receiverType, Id = id });

            base.SetReceivers(receivers);
        }
    }
}
