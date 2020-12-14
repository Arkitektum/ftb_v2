using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        protected override async Task CreateNotificationReceiverDatabaseStatus(string archiveReference, string senderId)
        {
            try
            {
                var distributionForm = await _dbUnitOfWork.DistributionForms.Get(Guid.Parse(FormData.hovedinnsendingsnummer.ToUpper()));
                var InitialArchiveReference = distributionForm.InitialArchiveReference;
                var receiverEntity = new NotificationReceiverEntity(InitialArchiveReference, ArchiveReference, Receivers[0].Id, ReceiverProcessStageEnum.Created, DateTime.Now);
                receiverEntity.PlanId = FormData.planid;
                receiverEntity.PlanNavn = FormData.planNavn;
                receiverEntity.Reply = FormData.beroertPart.kommentar;
                receiverEntity.ReceiverName = FormData.beroertPart.navn;
                receiverEntity.ReceiverAddress = FormatAddress(FormData.beroertPart.adresse.adresselinje1, FormData.beroertPart.adresse.postnr,FormData.beroertPart.adresse.poststed);
                receiverEntity.ReceiverEmail = FormData.beroertPart.epost;
                receiverEntity.SenderId = Sender.Id;

                await _tableStorage.InsertEntityRecordAsync<NotificationReceiverEntity>(receiverEntity);

                string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";
                var receiverLogEntity = new NotificationReceiverLogEntity(ArchiveReference, rowKey, Receivers[0].Id, ReceiverStatusLogEnum.Created);
                await _tableStorage.InsertEntityRecordAsync<NotificationReceiverLogEntity>(receiverLogEntity);

                _log.LogDebug($"Create submittal database status for {archiveReference}.");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error creating submittal record for archiveReference={archiveReference}.");
                throw;
            }
        }
    }
}
