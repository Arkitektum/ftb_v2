using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
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
                                                        IBlobOperations blobOperations,
                                                        ILogger<VarselOppstartPlanarbeidPrepareLogic> log,
                                                        DbUnitOfWork dbUnitOfWork,
                                                        IDecryptionFactory decryptionFactory) :
            base(repo, tableStorage, blobOperations, log, dbUnitOfWork, decryptionFactory)
        { }

        public override async Task<IEnumerable<SendQueueItem>> ExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
            var returnValue = await base.ExecuteAsync(submittalQueueItem);

            var initialArchiveReference = await GetInitialArchiveReferenceAsync(FormData.hovedinnsendingsnummer);
            _log.LogDebug($"{GetType().Name}: Got initialArchiveReference {initialArchiveReference} for distributionId {FormData.hovedinnsendingsnummer}");

            await MakePDFReplyPublicAccessible(submittalQueueItem.ArchiveReference, initialArchiveReference, FormData.beroertPart.navn);
            await CreateNotificationSenderDatabaseStatus(submittalQueueItem.ArchiveReference);
            
            return returnValue;
        }

        private async Task MakePDFReplyPublicAccessible(string neighboursArchiveReference, string initialArchiveReference, string partyName)
        {
            _log.LogDebug($"{GetType().Name}: Calling GetPublicBlobContainerName for ArchiveReference {initialArchiveReference}");
            var publicBlobContainer = _blobOperations.GetPublicBlobContainerName(initialArchiveReference);

            _log.LogDebug($"{GetType().Name}: Retrieved PDF for archiveReference {neighboursArchiveReference}");
            var PDFdoc = GetPDFReplyFromPrivateBlobStorageAsync(neighboursArchiveReference).Result;
            
            _log.LogDebug($"{GetType().Name}: Copying PDF from initialArchiveReference {initialArchiveReference} to PublicBlobStorage {publicBlobContainer} for archiveReference {neighboursArchiveReference}");
            await CopyPDFToPublicBlobStorage(PDFdoc, partyName, publicBlobContainer, neighboursArchiveReference);

            _log.LogDebug($"{GetType().Name}: Successfully copied PDF for archiveReference {neighboursArchiveReference} and party {partyName}");
        }

        public override void SetSender()
        {
            Enum.TryParse(this.FormData.beroertPart.partstype.kodeverdi, out ActorType senderType);
            string id;
            if (senderType.Equals(ActorType.Privatperson))
            {
                id = this.FormData.beroertPart.foedselsnummer;
            }
            else
            {
                id = this.FormData.beroertPart.organisasjonsnummer;
            }

            Sender = new Actor() { Id = id, Type = senderType };
        }

        public override void SetReceivers()
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

            var receivers = new List<Actor>();
            receivers.Add(new Actor() { Type = receiverType, Id = id });

            base.SetReceivers(receivers);
        }

        private async Task CreateNotificationSenderDatabaseStatus(string archiveReference)
        {
            try
            {
                _log.LogDebug($"{GetType().Name}: Creating NotificationSenderEntity database record for {archiveReference}");

                var InitialArchiveReference = await GetInitialArchiveReferenceAsync(FormData.hovedinnsendingsnummer);

                var senderEntity = new NotificationSenderEntity(InitialArchiveReference.ToLower(), ArchiveReference.ToLower(), Sender.Id, NotificationSenderProcessStageEnum.Created, DateTime.Now);
                senderEntity.PlanId = FormData.planid;
                senderEntity.PlanNavn = FormData.planNavn;
                senderEntity.Reply = FormData.beroertPart.kommentar;
                senderEntity.SenderName = FormData.beroertPart.navn;
                senderEntity.SenderPhone = FormData.beroertPart.telefon;
                senderEntity.SenderEmail = FormData.beroertPart.epost;
                senderEntity.ReceiverId = Receivers[0].Id;

                await _tableStorage.InsertEntityRecordAsync<NotificationSenderEntity>(senderEntity);

                string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";
                var senderLogEntity = new NotificationSenderLogEntity(ArchiveReference, rowKey, Sender.Id, NotificationSenderStatusLogEnum.Created);
                await _tableStorage.InsertEntityRecordAsync<NotificationSenderLogEntity>(senderLogEntity);

                _log.LogDebug($"Create NotificationSenderEntity database record for {archiveReference}.");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error creating submittal record for archiveReference={archiveReference}.");
                throw;
            }
        }
    }
}
