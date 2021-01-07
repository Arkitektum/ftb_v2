using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using Ftb_DbModels;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class DistributionSendLogic<T> : SendLogic<T>
    {
        private readonly IDistributionAdapter _distributionAdapter;
        protected IEnumerable<IPrefillData> PrefillSendData;

        public AltinnDistributionMessage DistributionMessage { get; set; }


        public DistributionSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IDistributionAdapter distributionAdapter, DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, dbUnitOfWork)
        {
            _distributionAdapter = distributionAdapter;
        }

        public override async Task<ReportQueueItem> ExecuteAsync(SendQueueItem sendQueueItem)
        {            
            _dbUnitOfWork.SetArchiveReference(sendQueueItem.ArchiveReference);

            //var returnReportQueueItem = await base.ExecuteAsync(sendQueueItem);
            await base.ExecuteAsync(sendQueueItem);

            this.Receiver = sendQueueItem.Receiver;

            if (Receiver == null || string.IsNullOrEmpty(Receiver.Id))
            {
                _dbUnitOfWork.LogEntries.AddError("Fant ikke personnummer/organisasjonsnummer");
                _dbUnitOfWork.LogEntries.AddErrorInternal($"ArchiveReference {sendQueueItem.ArchiveReference} - Fant ikke personnummer/organisasjonsnummer i Receiver", "AltinnPrefill");
                throw new Exception($"ArchiveReference {sendQueueItem.ArchiveReference} - Fant ikke personnummer/organisasjonsnummer i Receiver");
            }

            var returnReportQueueItem = new ReportQueueItem()
            {
                ArchiveReference = sendQueueItem.ArchiveReference,
                ReceiverLogPartitionKey = sendQueueItem.ReceiverLogPartitionKey,
                ReceiverSequenceNumber = sendQueueItem.ReceiverSequenceNumber,
                Sender = sendQueueItem.Sender,
                Receiver = sendQueueItem.Receiver
            };

            //Section below commented: when queue item is being processed in this "SendLogic", it means that it is to be sent out, even if it failed previously
            //Check state
            //this.State = await base.GetReceiverLastLogStatusAsync(sendQueueItem.ReceiverLogPartitionKey);

            //if (TrySendDistribution(this.State))
            //{
            //    return returnReportQueueItem;
            //}

            //if (returnReportQueueItem == null) //State machine thingy should handle this..
            //    return returnReportQueueItem;

            await MapFormDataToLogicData(sendQueueItem);

            var prefillData = PrefillSendData.FirstOrDefault();

            await CreateDistributionForm(prefillData);
            // NOTE:The distributionForm will not be saved if distribution sending fails

            await SendDistribution(sendQueueItem, prefillData);

            //TODO - Has been temp used for testing of ReservedReportee
            //if (sendQueueItem.Receiver.Id.Equals("910041126"))
            //{
            //    UpdateReceiverProcessOutcome(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverSequenceNumber, sendQueueItem.Receiver.Id, ReceiverProcessOutcomeEnum.ReservedReportee);
            //}

            return returnReportQueueItem;
        }

        private async Task SendDistribution(SendQueueItem sendQueueItem, IPrefillData prefillData)
        {
            _log.LogDebug("Sending distribution form with reference {0} for {1} - {2}", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference);
            var result = await _distributionAdapter.SendDistribution(DistributionMessage);

            //Remember "postdistribution metadata SendPrefillServiceV2 line 152 - 162

            //Log results
            LogPrefillProcessing(result, prefillData);
            LogDistributionProcessingResults(result, prefillData);

            //if (result.Where(r => r.Step == DistributionStep.Failed || r.Step == DistributionStep.UnkownErrorOccurred || r.Step == DistributionStep.UnableToReachReceiver).Any())
            if (result.Where(r => r.Step == DistributionStep.Failed || r.Step == DistributionStep.UnkownErrorOccurred).Any())
            {
                _log.LogError("Sending distribution form with reference {0} for {1} - {2} failed", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference);
                //distributionForm.DistributionStatus = DistributionStatus.error;
                //distributionForm.ErrorMessage = "Send manuelt";

                await UpdateReceiverProcessOutcomeAsync(sendQueueItem.ArchiveReference,
                                                        sendQueueItem.ReceiverSequenceNumber,
                                                        sendQueueItem.Receiver.Id,
                                                        ReceiverProcessOutcomeEnum.Failed);

                await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey,
                                                    sendQueueItem.Receiver.Id,
                                                    DistributionReceiverStatusLogEnum.CorrespondenceSendingFailed);

                throw new DistributionSendExeception(prefillData.InitialExternalSystemReference,
                                                        prefillData.ExternalSystemReference,
                                                        DistributionMessage.DistributionFormReferenceId,
                                                        "CorrespondenceSendingFailed");
            }
            else if (result.Where(r => r.Step == DistributionStep.ReservedReportee).Any())
            {
                _log.LogInformation("Sending distribution form with reference {0} for {1} - {2} was prevented due to ReservedReportee", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference);
                await UpdateReceiverProcessOutcomeAsync(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverSequenceNumber, sendQueueItem.Receiver.Id, ReceiverProcessOutcomeEnum.ReservedReportee);
                await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey, sendQueueItem.Receiver.Id, DistributionReceiverStatusLogEnum.ReservedReportee);
            }
            else if (result.Where(r => r.Step == DistributionStep.UnableToReachReceiver).Any())
            {
                _log.LogInformation("Sending distribution form with reference {0} for {1} - {2} failed due to unable to reach receiver (non-existing)", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference);
                await UpdateReceiverProcessOutcomeAsync(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverSequenceNumber, sendQueueItem.Receiver.Id, ReceiverProcessOutcomeEnum.Failed);
                await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey, sendQueueItem.Receiver.Id, DistributionReceiverStatusLogEnum.CorrespondenceSendingFailed);
            }
            else
            {
                _log.LogInformation("Sending distribution form with reference {0} for {1} - {2} was ok. Steps {3}", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference, result.Select(x => x.Step).ToList());
                _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefillData.InitialExternalSystemReference} - Distribusjon behandling ferdig");

                await UpdateReceiverProcessOutcomeAsync(sendQueueItem.ArchiveReference,
                                                        sendQueueItem.ReceiverSequenceNumber,
                                                        sendQueueItem.Receiver.Id,
                                                        ReceiverProcessOutcomeEnum.Sent);

                await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey,
                                                    sendQueueItem.Receiver.Id,
                                                    DistributionReceiverStatusLogEnum.CorrespondenceSent);
            }

            await UpdateReceiverProcessStageAsync(sendQueueItem.ArchiveReference,
                                                    sendQueueItem.ReceiverSequenceNumber,
                                                    sendQueueItem.Receiver.Id,
                                                    DistributionReceiverProcessStageEnum.Distributed);
        }

        private async Task CreateDistributionForm(IPrefillData prefillData)
        {
            //Which sendData to use
            prefillData.InitialExternalSystemReference = DistributionMessage.DistributionFormReferenceId.ToString();
            _log.LogDebug("Creates distribution form with reference {0} for {1} - {2}", prefillData.InitialExternalSystemReference, ArchiveReference, prefillData.ExternalSystemReference);

            _dbUnitOfWork.DistributionForms.Add(new DistributionForm()
            {
                Id = DistributionMessage.DistributionFormReferenceId,
                InitialExternalSystemReference = prefillData.InitialExternalSystemReference,
                ExternalSystemReference = prefillData.ExternalSystemReference,
                DistributionType = prefillData.PrefillFormName
            });

            var distributionForm = (await _dbUnitOfWork.DistributionForms.GetAll())
                                        .Where(d => d.Id == DistributionMessage.DistributionFormReferenceId).FirstOrDefault();

            //Creates combined distribution data structure -- BØR GJERAST ANLEIS.... BORT MED SEG!
            foreach (var combinedCandidate in PrefillSendData.Where(p => p != prefillData))
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

            //PersistPrefill(sendQueueItem);

            _dbUnitOfWork.LogEntries.AddInfo($"Starter distribusjon med søknadsystemsreferanse {prefillData.ExternalSystemReference}");
            _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefillData.InitialExternalSystemReference} - Distribusjon av {prefillData.PrefillFormName} til tjeneste {prefillData.PrefillServiceCode}/{prefillData.PrefillServiceEditionCode}");
            distributionForm.SubmitAndInstantiatePrefilled = DateTime.Now;

            //return distributionForm;
        }

        private async Task MapFormDataToLogicData(SendQueueItem sendQueueItem)
        {
            _log.LogDebug("Maps prefill data for {0}", sendQueueItem.Receiver.Id);
            MapPrefillData(sendQueueItem.Receiver.Id);
            await MapDistributionMessage();

            await UpdateReceiverProcessStageAsync(sendQueueItem.ArchiveReference, sendQueueItem.ReceiverSequenceNumber, sendQueueItem.Receiver.Id, DistributionReceiverProcessStageEnum.Distributing);
            await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey, sendQueueItem.Receiver.Id, DistributionReceiverStatusLogEnum.PrefillCreated);
        }

        private void LogDistributionProcessingResults(IEnumerable<DistributionResult> distributionResults, IPrefillData prefill)
        {
            var correspondenceResults = distributionResults.Where(p => p.DistributionComponent == DistributionComponent.Correspondence).ToList();

            foreach (var correspondenceResult in correspondenceResults)
            {
                switch (correspondenceResult.Step)
                {
                    case DistributionStep.PayloadCreated:
                        break;
                    case DistributionStep.Sent:
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn kvittering for preutfylling til {Receiver.PresentationId} er OK");
                        _dbUnitOfWork.LogEntries.AddInfoInternal($"Dist id {prefill.InitialExternalSystemReference} - Distribusjon/correspondence komplett", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {prefill.PrefillFormName} laget til ({Receiver.PresentationId}), Altinn kvitteringsid: TODO LEGG INN receiptExternal.ReceiptId");
                      
                        break;
                    case DistributionStep.Failed:
                        _dbUnitOfWork.LogEntries.AddError($"Dist id {prefill.InitialExternalSystemReference} - Feil ved Altinn utsendelse av melding til {Receiver.PresentationId} ");
                        _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {prefill.InitialExternalSystemReference} - Distribusjon/correspondence Altinn feil for bruker {Receiver.PresentationId}: {correspondenceResult.Message}", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {prefill.PrefillFormName} pefill OK men correspondence feilet til ({Receiver.PresentationId}), Altinn prefill kvitteringsid: TODO LEGG TIL receiptExternal.ReceiptId");
                        break;
                    case DistributionStep.UnkownErrorOccurred:
                        _dbUnitOfWork.LogEntries.AddErrorInternal($"Dist id {prefill.InitialExternalSystemReference} - Altinn correspondence WS avvik: {correspondenceResult.Message}", "Correspondence");
                        _dbUnitOfWork.LogEntries.AddError("Feil ved utsedning av melding til Altinn bruker. Preutfylt skjema ligger nå uten tilhørende melding for tjenesten");
                        _dbUnitOfWork.LogEntries.AddInfo($"Altinn {prefill.PrefillFormName} pefill OK men correspondence feilet til ({Receiver.PresentationId}), Altinn prefill kvitteringsid: TODO LEGG TIL receiptExternal.ReceiptId");
                        break;
                    case DistributionStep.ReservedReportee:
                        break;
                    case DistributionStep.UnableToReachReceiver:
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task LogPrefillProcessing(IEnumerable<DistributionResult> distributionResults, IPrefillData prefill)
        {
            var prefillResults = distributionResults.Where(p => p.DistributionComponent == DistributionComponent.Prefill).ToList();
            foreach (var prefillResult in prefillResults)
            {
                switch (prefillResult.Step)
                {
                    case DistributionStep.PayloadCreated:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefill.InitialExternalSystemReference} - Sender prefill kall til Altinn");
                        break;
                    case DistributionStep.Sent:
                        _dbUnitOfWork.LogEntries.AddInfo($"Dist id {prefill.InitialExternalSystemReference} - Prefill sendt til Altinn");
                        break;
                    case DistributionStep.Failed:
                        _dbUnitOfWork.LogEntries.AddError($"Dist id {prefill.InitialExternalSystemReference} - {prefillResult.Message}");
                        break;
                    case DistributionStep.UnkownErrorOccurred:
                        _dbUnitOfWork.LogEntries.AddError($"Unntak ved Altinn utsendelse av {prefill.PrefillFormName} til {Receiver.PresentationId}");
                        break;
                    case DistributionStep.ReservedReportee:
                        _dbUnitOfWork.LogEntries.AddInfoInternal($"Dist id {prefill.InitialExternalSystemReference} - Nabovarsel print pga. reservasjon", "Prefill");
                        break;
                    case DistributionStep.UnableToReachReceiver:
                        _dbUnitOfWork.LogEntries.AddError($"Mottaker kunne ikke nås i Altinn {Receiver.PresentationId} : {prefillResult.Message}");
                        var df = await _dbUnitOfWork.DistributionForms.Get(prefill.InitialExternalSystemReference);
                        df.DistributionStatus = DistributionStatus.error;
                        df.ErrorMessage = prefillResult.Message;

                        break;
                    default:
                        break;
                }
            }
        }

        //protected virtual async Task PersistPrefill(SendQueueItem sendQueueItem)
        //{
        //    var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
        //    _log.LogDebug($"{GetType().Name}: PersistPrefill for archiveReference {sendQueueItem.ArchiveReference}....");
        //    _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(DistributionMessage.PrefilledXmlDataString), metaData);
        //    await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusLogEnum.PrefillPersisted);
        //}

        //protected virtual async Task PersistMessage(SendQueueItem sendQueueItem)
        //{
        //    var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("DistributionMessageReceiver", sendQueueItem.Receiver.Id) };
        //    _log.LogDebug($"{GetType().Name}: PersistMessage for archiveReference {sendQueueItem.ArchiveReference}....");
        //    _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Message-{Guid.NewGuid()}", Encoding.Default.GetBytes(SerializeUtil.Serialize(DistributionMessage.NotificationMessage.MessageData)), metaData);
        //   await AddToReceiverProcessLogAsync(sendQueueItem.ReceiverLogPartitionKey, sendQueueItem.Receiver.Id, ReceiverStatusLogEnum.PrefillPersisted);
        //}

        protected abstract void MapPrefillData(string receiverId);

        protected virtual async Task MapDistributionMessage()
        {
        }

        //protected abstract void AddAttachmentsToDistribution(SendQueueItem sendQueueItem);
    }
}