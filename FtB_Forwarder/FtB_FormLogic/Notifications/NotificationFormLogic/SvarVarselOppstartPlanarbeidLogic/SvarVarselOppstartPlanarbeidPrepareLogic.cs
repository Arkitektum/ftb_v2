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
            await base.ExecuteAsync(submittalQueueItem);

            var initialArchiveReference = await GetInitialArchiveReferenceAsync(FormData.hovedinnsendingsnummer);
            _log.LogDebug($"{GetType().Name}: Got initialArchiveReference {initialArchiveReference} for distributionId {FormData.hovedinnsendingsnummer}");

            await MakePDFReplyPublicAccessible(submittalQueueItem.ArchiveReference, initialArchiveReference, FormData.beroertPart.navn);
            await CreateNotificationSenderDatabaseStatus(submittalQueueItem.ArchiveReference);
            
            //At the end of processing this method, processing is completed for this schema (FormLogic), 
            //thus returning null (meaning put no element on the queue)
            return null;
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
            Enum.TryParse(EnumExtentions.GetValueFromDescription<ActorType>(FormData.beroertPart.partstype.kodeverdi).ToString(), out ActorType senderType);
            string id = (senderType.Equals(ActorType.Privatperson) ? FormData.beroertPart.foedselsnummer : FormData.beroertPart.organisasjonsnummer);
            Sender = new Actor() { Id = id, Type = senderType };
            _log.LogDebug($"SetSender: {FormData.beroertPart.navn}, Id: {id}");
        }

        public override void SetReceivers()
        {
            Enum.TryParse(EnumExtentions.GetValueFromDescription<ActorType>(FormData.forslagsstiller.partstype.kodeverdi).ToString(), out ActorType receiverType);
            string id = (receiverType.Equals(ActorType.Privatperson) ? FormData.forslagsstiller.foedselsnummer : FormData.forslagsstiller.organisasjonsnummer);
            var receivers = new List<Actor>();
            receivers.Add(new Actor() { Type = receiverType, Id = id });
            base.SetReceivers(receivers);
            _log.LogDebug($"SetReceiver: {FormData.forslagsstiller.navn}, Id: {id}");
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

        protected override Guid GetHovedinnsendingsNummer()
        {
            if (Guid.TryParse(FormData.hovedinnsendingsnummer, out var newGuid))
                return newGuid;
            throw new ArgumentOutOfRangeException($"Illegal distribution id. Could not parse {FormData.hovedinnsendingsnummer}");
        }

    }
}
