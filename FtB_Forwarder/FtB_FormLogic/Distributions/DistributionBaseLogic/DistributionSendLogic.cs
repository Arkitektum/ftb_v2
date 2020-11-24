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
using System.Linq;using System.Text;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class DistributionSendLogic<T> : SendLogic<T>
    {
        private readonly IDistributionAdapter _distributionAdapter;
        protected IEnumerable<IPrefillData> prefillSendData;

        public AltinnDistributionMessage DistributionMessage { get; set; }


        public DistributionSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log, IDistributionAdapter distributionAdapter, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, tableStorageOperations, log, dbUnitOfWork)
        {
            _distributionAdapter = distributionAdapter;
        }

        public override async Task<ReportQueueItem> Execute(SendQueueItem sendQueueItem)
        {
            _log.LogDebug("_dbUnitOfWork hash {0}", _dbUnitOfWork.GetHashCode());
            var returnReportQueueItem = await base.Execute(sendQueueItem);
            _log.LogDebug("Maps prefill data for {0}", sendQueueItem.Receiver.Id);
            MapPrefillData(sendQueueItem.Receiver.Id);
            await MapDistributionMessage();
            AddReceiverProcessStatus(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillCreated);
            //Which sendData to use
            var prefillData = prefillSendData.FirstOrDefault();


            prefillData.InitialExternalSystemReference = DistributionMessage.DistributionFormReferenceId.ToString();
            _log.LogDebug("Creates distribution form with reference {0} for {1} - {2}", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference);

            _dbUnitOfWork.DistributionForms.Add(new DistributionForm() { Id = DistributionMessage.DistributionFormReferenceId, ExternalSystemReference = prefillData.ExternalSystemReference, InitialExternalSystemReference = prefillData.InitialExternalSystemReference, DistributionType = prefillData.PrefillFormName });
            var distributionForm = (await _dbUnitOfWork.DistributionForms.Get())
                                        .Where(d => d.Id == DistributionMessage.DistributionFormReferenceId).FirstOrDefault();

            //Creates combined distribution data structure -- BØR GJERAST ANLEIS.... BORT MED SEG!
            foreach (var combinedCandidate in prefillSendData.Where(p => p != prefillData))
            {
                var childDistribution = new DistributionForm()
                {
                    Id = Guid.NewGuid(),
                    InitialExternalSystemReference = prefillData.InitialExternalSystemReference,
                    ExternalSystemReference = combinedCandidate.ExternalSystemReference,
                    DistributionReference = DistributionMessage.DistributionFormReferenceId,
                };
                _dbUnitOfWork.DistributionForms.Add(childDistribution);
                _dbUnitOfWork.LogEntries.AddInfo($"Distribusjon med søknadsystemsreferanse {prefillData.InitialExternalSystemReference} kombinert med {childDistribution.ExternalSystemReference}");
                _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefillData.InitialExternalSystemReference} kombinert med {childDistribution.Id}", "Info");
            }

            PersistPrefill(sendQueueItem);

            _dbUnitOfWork.LogEntries.AddInfo($"Starter distribusjon med søknadsystemsreferanse {prefillData.ExternalSystemReference}");
            _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefillData.InitialExternalSystemReference} - Distribusjon av {prefillData.PrefillFormName} til tjeneste {prefillData.PrefillServiceCode}/{prefillData.PrefillServiceEditionCode}");
            distributionForm.SubmitAndInstantiatePrefilled = DateTime.Now;

            if (Receiver == null || string.IsNullOrEmpty(Receiver.Id))
            {
                _dbUnitOfWork.LogEntries.AddError("Fant ikke personnummer/organisasjonsnummer");
                _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {prefillData.InitialExternalSystemReference} - Fant ikke personnummer/organisasjonsnummer i Receiver", "AltinnPrefill");
            }
            else
            {
                var result = await _distributionAdapter.SendDistribution(DistributionMessage);

                //Remember "postdistribution metadata SendPrefillServiceV2 line 152 - 162

                //Log results
                LogPrefillProcessing(result, prefillData);
                LogDistributionProcessingResults(result, prefillData);

                if (result.Where(r => r.Step == DistriutionStep.Failed || r.Step == DistriutionStep.UnkownErrorOccurred || r.Step == DistriutionStep.UnableToReachReceiver).Any())
                {
                    distributionForm.DistributionStatus = DistributionStatus.error;
                    distributionForm.ErrorMessage = "Send manuelt";
                }


                _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefillData.InitialExternalSystemReference} - Distribusjon behandling ferdig");

                //Use result of SendDistribution to update receiver entity
                AddReceiverProcessStatus(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusEnum.CorrespondenceSent);
            }
            ////TODO - Remove: for TEST
            //AddReceiverProcessStatus(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusEnum.DigitalDisallowment);
            //_log.LogDebug($"Legger {sendQueueItem.Receiver.Id} til som denier");

            return returnReportQueueItem;
        }

        private void LogDistributionProcessingResults(IEnumerable<DistributionResult> distributionResults, IPrefillData prefill)
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
                        _dbUnitOfWork.LogEntries.AddInfoInternal($"Dist id {prefill.InitialExternalSystemReference} - Distribusjon/correspondence komplett", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {prefill.PrefillFormName} laget til ({Receiver.PresentationId}), Altinn kvitteringsid: TODO LEGG INN receiptExternal.ReceiptId");
                        /*
                    _logEntryService.Save(new LogEntry(distributionServiceFormData.ArchiveReference, $"Altinn kvittering for preutfylling til {reporteePresentation} er OK", "Info"));
                    _logEntryService.Save(new LogEntry(distributionServiceFormData.ArchiveReference, $"Dist id {prefillFormData.GetPrefillKey()} - Distribusjon/correspondence komplett", LogEntry.Info, LogEntry.InternalMsg));                         
                         */


                        break;
                    case DistriutionStep.Failed:
                        _dbUnitOfWork.LogEntries.AddError($"Dist id {prefill.InitialExternalSystemReference} - Feil ved Altinn utsendelse av melding til {Receiver.PresentationId} ");
                        _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {prefill.InitialExternalSystemReference} - Distribusjon/correspondence Altinn feil for bruker {Receiver.PresentationId}: {correspondenceResult.Message}", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {prefill.PrefillFormName} pefill OK men correspondence feilet til ({Receiver.PresentationId}), Altinn prefill kvitteringsid: TODO LEGG TIL receiptExternal.ReceiptId");
                        break;
                    case DistriutionStep.UnkownErrorOccurred:
                        _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {prefill.InitialExternalSystemReference} - Altinn correspondence WS avvik: {correspondenceResult.Message}", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddError("Feil ved utsedning av melding til Altinn bruker. Preutfylt skjema ligger nå uten tilhørende melding for tjenesten");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {prefill.PrefillFormName} pefill OK men correspondence feilet til ({Receiver.PresentationId}), Altinn prefill kvitteringsid: TODO LEGG TIL receiptExternal.ReceiptId");
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
        private void LogPrefillProcessing(IEnumerable<DistributionResult> distributionResults, IPrefillData prefill)
        {
            var prefillResults = distributionResults.Where(p => p.DistributionComponent == DistributionComponent.Prefill).ToList();
            foreach (var prefillResult in prefillResults)
            {
                switch (prefillResult.Step)
                {
                    case DistriutionStep.PayloadCreated:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefill.InitialExternalSystemReference} - Sender prefill kall til Altinn");
                        break;
                    case DistriutionStep.Sent:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefill.InitialExternalSystemReference} - Prefill sendt til Altinn");
                        break;
                    case DistriutionStep.Failed:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefill.InitialExternalSystemReference} - Sender prefill kall til Altinn");
                        break;
                    case DistriutionStep.UnkownErrorOccurred:
                        _dbUnitOfWork.LogEntries.AddInfo($"Unntak ved Altinn utsendelse av {prefill.PrefillFormName} til {Receiver.PresentationId}", "Prefill");
                        break;
                    case DistriutionStep.ReservedReportee:
                        _dbUnitOfWork.LogEntries.AddInfoInternal($"Dist id {prefill.InitialExternalSystemReference} - Nabovarsel print pga. reservasjon", "Prefill");
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
            AddReceiverProcessStatus(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillPersisted);
        }

        protected virtual void PersistMessage(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("DistributionMessageReceiver", sendQueueItem.Receiver.Id) };
            _log.LogDebug($"{GetType().Name}: PersistMessage for archiveReference {sendQueueItem.ArchiveReference}....");
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Message-{Guid.NewGuid()}", Encoding.Default.GetBytes(SerializeUtil.Serialize(DistributionMessage.NotificationMessage.MessageData)), metaData);
            AddReceiverProcessStatus(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillPersisted);
        }

        protected abstract void MapPrefillData(string receiverId);

        protected virtual async Task MapDistributionMessage()
        {
            _dbUnitOfWork.LogEntries.AddInfo($"Starter distribusjon med søknadsystemsreferanse {prefillSendData.FirstOrDefault()?.ExternalSystemReference}");
        }

        //protected abstract void AddAttachmentsToDistribution(SendQueueItem sendQueueItem);
    }
}