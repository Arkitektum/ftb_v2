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
            await CreateNotificationReceiverDatabaseStatus(submittalQueueItem.ArchiveReference);
            
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

        private async Task CreateNotificationReceiverDatabaseStatus(string archiveReference)
        {
            try
            {
                _log.LogDebug($"{GetType().Name}: Creating NotificationReceiverEntity database record for {archiveReference}");

                var InitialArchiveReference = await GetInitialArchiveReferenceAsync(FormData.hovedinnsendingsnummer);

                var receiverEntity = new NotificationReceiverEntity(InitialArchiveReference.ToLower(), ArchiveReference.ToLower(), Receivers[0].Id, NotificationReceiverProcessStageEnum.Created, DateTime.Now);
                receiverEntity.PlanId = FormData.planid;
                receiverEntity.PlanNavn = FormData.planNavn;
                receiverEntity.Reply = FormData.beroertPart.kommentar;
                receiverEntity.ReceiverName = FormData.beroertPart.navn;
                receiverEntity.ReceiverPhone = FormData.beroertPart.telefon;
                receiverEntity.ReceiverEmail = FormData.beroertPart.epost;
                receiverEntity.SenderId = Sender.Id;

                await _tableStorage.InsertEntityRecordAsync<NotificationReceiverEntity>(receiverEntity);

                string rowKey = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}";
                var receiverLogEntity = new NotificationReceiverLogEntity(ArchiveReference, rowKey, Receivers[0].Id, ReceiverStatusLogEnum.Created);
                await _tableStorage.InsertEntityRecordAsync<NotificationReceiverLogEntity>(receiverLogEntity);

                _log.LogDebug($"Create NotificationReceiverEntity database record for {archiveReference}.");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error creating submittal record for archiveReference={archiveReference}.");
                throw;
            }
        }
    }
}
