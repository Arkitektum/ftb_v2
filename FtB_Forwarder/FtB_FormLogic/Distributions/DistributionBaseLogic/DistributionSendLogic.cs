using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using Ftb_DbModels;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_FormLogic
{
    public abstract class DistributionSendLogic<T> : SendLogic<T>
    {
        private readonly IDistributionAdapter _distributionAdapter;
        private readonly ISendData sendData;

        public AltinnDistributionMessage DistributionMessage { get; set; }


        public DistributionSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IDistributionAdapter distributionAdapter, ISendData sendData, DbUnitOfWork dbUnitOfWork) : base(repo, tableStorage, log, dbUnitOfWork)
        {
            _distributionAdapter = distributionAdapter;
            this.sendData = sendData;
        }

        public override ReportQueueItem Execute(SendQueueItem sendQueueItem)
        {
            var returnReportQueueItem = base.Execute(sendQueueItem);
            MapPrefillData(sendQueueItem.Receiver.Id);
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillCreated);

            //DistributionForm dForm = _formMetadataService.InsertDistributionForm(archivereferance, prefillFormData.GetPrefillKey(), prefillFormData.GetPrefillOurReference(), altinnForm.GetName());
            //_dbUnitOfWork.DistributionForms.Add(new DistributionForm() { this.sendData.PrefillServiceEditionCode )

            //prefillData = 
            PersistPrefill(sendQueueItem);

            //Generate distributionForms
            /*
                foreach (var ownedproperty in nabovarselSvar.nabo.gjelderNaboeiendom)
                {
                    requestingSystemReferences.Add(ownedproperty.sluttbrukersystemVaarReferanse);
                }

            -----
                for (int i = 1; i < neighborReferenceIdList.Count; i++)
                {
                    DistributionForm dFormDummy = _formMetadataService.InsertDistributionForm(archivereferance, prefillFormData.GetPrefillKey(), neighborReferenceIdList[i], altinnForm.GetName());
                    dFormDummy.DistributionReference = dForm.Id;
                    _formMetadataService.SaveDistributionForm(dFormDummy);
                    _logEntryService.Save(new LogEntry(archivereferance, $"Distribusjon med søknadsystemsreferanse {prefillFormData.GetPrefillOurReference()} kombinert med {neighborReferenceIdList[i]}", LogEntry.Info, LogEntry.ExternalMsg));
                    _logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} kombinert med {dFormDummy.Id.ToString()}", "Info", true));
                }
             */

            _dbUnitOfWork.LogEntries.AddInfo($"Starter distribusjon med søknadsystemsreferanse {sendData.ExternalSystemSubReference}");
            _dbUnitOfWork.LogEntries.AddInfo($"Dist id {sendData.ExternalSystemMainReference} - Distribusjon av {sendData.PrefillFormName} til tjeneste {sendData.PrefillServiceCode}/{sendData.PrefillServiceEditionCode}");

            if (Receiver == null && string.IsNullOrEmpty(Receiver.Id))
            {
                _dbUnitOfWork.LogEntries.AddError("Fant ikke personnummer/organisasjonsnummer");
                _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {sendData.ExternalSystemMainReference} - Fant ikke personnummer/organisasjonsnummer i Receiver", "AltinnPrefill");
            }
            var result = _distributionAdapter.SendDistribution(DistributionMessage);

            //Remember "postdistribution metadata SendPrefillServiceV2 line 152 - 162

            //Log results
            LogPrefillProcessing(result);
            LogDistributionProcessingResults(result);

            _dbUnitOfWork.LogEntries.AddInfo($"Dist id {sendData.ExternalSystemMainReference} - Distribusjon behandling ferdig");

            //Use result of SendDistribution to update receiver entity
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.CorrespondenceSent);

            return returnReportQueueItem;
        }

        private void LogDistributionProcessingResults(IEnumerable<DistributionResult> distributionResults)
        {
            var correspondenceResults = distributionResults.Where(p => p.DistributionComponent == DistributionComponent.Correspondence).ToList();

            foreach (var correspondenceResult in correspondenceResults)
            {
                switch (correspondenceResult.Step)
                {
                    case DistriutionStep.PayloadCreated:
                        break;
                    case DistriutionStep.Sent:
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn kvittering for preutfylling til {Receiver.PresentationId} er OK");
                        _dbUnitOfWork.LogEntries.AddInfoInternal($"Dist id {sendData.ExternalSystemMainReference} - Distribusjon/correspondence komplett", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {sendData.PrefillFormName} laget til ({Receiver.PresentationId}), Altinn kvitteringsid: TODO LEGG INN receiptExternal.ReceiptId");
                        /*
                    _logEntryService.Save(new LogEntry(distributionServiceFormData.ArchiveReference, $"Altinn kvittering for preutfylling til {reporteePresentation} er OK", "Info"));
                    _logEntryService.Save(new LogEntry(distributionServiceFormData.ArchiveReference, $"Dist id {prefillFormData.GetPrefillKey()} - Distribusjon/correspondence komplett", LogEntry.Info, LogEntry.InternalMsg));                         
                         */


                        break;
                    case DistriutionStep.Failed:
                        _dbUnitOfWork.LogEntries.AddError($"Dist id {sendData.ExternalSystemMainReference} - Feil ved Altinn utsendelse av melding til {Receiver.PresentationId} ");
                        _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {sendData.ExternalSystemMainReference} - Distribusjon/correspondence Altinn feil for bruker {Receiver.PresentationId}: {correspondenceResult.Message}", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {sendData.PrefillFormName} pefill OK men correspondence feilet til ({Receiver.PresentationId}), Altinn prefill kvitteringsid: TODO LEGG TIL receiptExternal.ReceiptId");
                        break;
                    case DistriutionStep.UnkownErrorOccurred:
                        _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {sendData.ExternalSystemMainReference} - Altinn correspondence WS avvik: {correspondenceResult.Message}", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddError("Feil ved utsedning av melding til Altinn bruker. Preutfylt skjema ligger nå uten tilhørende melding for tjenesten");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {sendData.PrefillFormName} pefill OK men correspondence feilet til ({Receiver.PresentationId}), Altinn prefill kvitteringsid: TODO LEGG TIL receiptExternal.ReceiptId");
                        break;
                    case DistriutionStep.ReservedReportee:
                        break;
                    case DistriutionStep.UnableToReachReceiver:
                        break;
                    default:
                        break;
                }
            }
        }
        private void LogPrefillProcessing(IEnumerable<DistributionResult> distributionResults)
        {
            var prefillResults = distributionResults.Where(p => p.DistributionComponent == DistributionComponent.Prefill).ToList();
            foreach (var prefillResult in prefillResults)
            {
                switch (prefillResult.Step)
                {
                    case DistriutionStep.PayloadCreated:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {sendData.ExternalSystemMainReference} - Sender prefill kall til Altinn");
                        break;
                    case DistriutionStep.Sent:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {sendData.ExternalSystemMainReference} - Prefill sendt til Altinn");
                        break;
                    case DistriutionStep.Failed:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {sendData.ExternalSystemMainReference} - Sender prefill kall til Altinn");
                        break;
                    case DistriutionStep.UnkownErrorOccurred:
                        _dbUnitOfWork.LogEntries.AddInfo($"Unntak ved Altinn utsendelse av {sendData.PrefillFormName} til {Receiver.PresentationId}", "Prefill");
                        break;
                    case DistriutionStep.ReservedReportee:
                        _dbUnitOfWork.LogEntries.AddInfoInternal($"Dist id {sendData.ExternalSystemMainReference} - Nabovarsel print pga. reservasjon", "Prefill");
                        break;
                    case DistriutionStep.UnableToReachReceiver:
                        break;
                    default:
                        break;
                }
            }
        }

        protected virtual void PersistPrefill(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
            _log.LogDebug($"{GetType().Name}: PersistPrefill for archiveReference {sendQueueItem.ArchiveReference}....");
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(DistributionMessage.PrefilledXmlDataString), metaData);
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillPersisted);
        }

        protected virtual void PersistMessage(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("DistributionMessageReceiver", sendQueueItem.Receiver.Id) };
            _log.LogDebug($"{GetType().Name}: PersistMessage for archiveReference {sendQueueItem.ArchiveReference}....");
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Message-{Guid.NewGuid()}", Encoding.Default.GetBytes(SerializeUtil.Serialize(DistributionMessage.NotificationMessage.MessageData)), metaData);
            UpdateReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillPersisted);
        }

        //protected virtual IEnumerable<AltinnDistributionResult> SendPrefill(SendQueueItem sendQueueItem)
        //{
        //    var distributionResults = _distributionAdapter.SendDistribution(DistributionMessage);
        //    //switch (prefillResult.ResultType)
        //    //{
        //    //    case PrefillResultType.Ok:
        //    //        UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.PrefillSent));
        //    //        break;
        //    //    case PrefillResultType.UnkownErrorOccured:
        //    //        break;
        //    //    case PrefillResultType.ReservedReportee:
        //    //        break;
        //    //    case PrefillResultType.UnableToReachReceiver:
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}

        //    return distributionResults;
        //}

        //protected virtual void Distribute(SendQueueItem sendQueueItem, string prefillReferenceId)
        //{
        //    // Validate if receiver info is sufficient

        //    // Decrypt

        //    // Create distributionform 
        //    var distributionFormId = this.PrefillData.DistributionFormId;

        //    // Map  from prefill-data to prefillFormTask
        //    // Send using prefill service

        //    // _distributionAdapter.SendDistribution(eitEllerAnnaObjekt);

        //    // Finally persist distributionform..  and maybe a list of logentries??            

        //    UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.StorageRowKey, ReceiverStatusEnum.CorrespondenceSent));
        //}
        protected abstract void MapPrefillData(string receiverId);
    }
}

