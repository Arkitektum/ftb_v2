using Altinn.Common;
using Altinn.Common.Exceptions;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_FormLogic;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FtB_ProcessStrategies
{
    public class ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor
    {
        private readonly IBlobOperations _blobOperations;
        private readonly ITableStorage _tableStorage;
        private readonly INotificationAdapter _notificationAdapter;
        private readonly IHtmlUtils _htmlUtils;
        private readonly HtmlToPdfConverterHttpClient _htmlToPdfConverterHttpClient;
        private readonly ILogger _log;

        public ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor(IBlobOperations blobOperations,
                                                                  ITableStorage tableStorage,
                                                                  INotificationAdapter notificationAdapter,
                                                                  IHtmlUtils htmlUtils,
                                                                  HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient,
                                                                  ILogger<ReportSvarPaaVarselOmOppstartAvPlanarbeidProcessor> log)
        {
            _blobOperations = blobOperations;
            _tableStorage = tableStorage;
            _notificationAdapter = notificationAdapter;
            _htmlUtils = htmlUtils;
            _htmlToPdfConverterHttpClient = htmlToPdfConverterHttpClient;
            _log = log;
        }

        public async Task ExecuteProcessingStrategyAsync()
        {
            _log.LogDebug("Getting distributionSubmittalEntities..");
            IEnumerable<DistributionSubmittalEntity> distributionSubmittalEntities = await GetDistributionSubmittalEntities();

            _log.LogDebug($"Number of distributionSubmittalEntities found: {distributionSubmittalEntities.Count()}");

            foreach (var distributionSubmittalEntity in distributionSubmittalEntities)
            {
                var answerToSubmitter = await GetAnswersFromSubmittalReceivers(distributionSubmittalEntity);
                _log.LogDebug($"Number of answers for {distributionSubmittalEntity.PartitionKey} found: {((answerToSubmitter == null) || (answerToSubmitter.Receivers.Count() == 0) ? "0" : answerToSubmitter.Receivers.Count().ToString())}");

                if (answerToSubmitter != null)
                {
                    var publicBlobContainer = _blobOperations.GetPublicBlobContainerName(answerToSubmitter.InitialArchiveReference.ToLower());
                    _log.LogDebug($"Reporting PDF replies to submitter for archiveReference {answerToSubmitter.InitialArchiveReference }");
                    await SendReceiptToSubmitterWhenSomeReceiversHaveRepliedAsync(answerToSubmitter, publicBlobContainer);
                }
            }
        }


        private IEnumerable<AttachmentBinary> ConvertBlobsToAltinnAttachments(IEnumerable<(byte[], string)> blobs)
        {
            var att = new List<AttachmentBinary>();
            foreach (var blob in blobs)
            {
                att.Add(new AttachmentBinary() { BinaryContent = blob.Item1, Filename = "Svar.pdf", AttachmentTypeName = "PDF", 
                                                 Type = "application/pdf", Name = "Svar på varsel om oppstart av planarbeid" });
            }

            return att;
        }

        private async Task<IEnumerable<DistributionSubmittalEntity>> GetDistributionSubmittalEntities()
        {
            var distributionSubmittalEntitiesCompleted = _tableStorage.GetTableEntitiesWithStatusFilter<DistributionSubmittalEntity>(Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed));
            var distributionSubmittalEntitiesReportingInProgress = _tableStorage.GetTableEntitiesWithStatusFilter<DistributionSubmittalEntity>(Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.ReportingInProgress));

            IEnumerable<DistributionSubmittalEntity> distributionSubmittalEntities = distributionSubmittalEntitiesCompleted.Select(x => x).Concat(distributionSubmittalEntitiesReportingInProgress.Select(y => y));
            return distributionSubmittalEntities;
        }

        private async Task<SvarPaaVarselOmOppstartAvPlanarbeidModel> GetAnswersFromSubmittalReceivers(DistributionSubmittalEntity distributionSubmittalEntity)
        {
            var filters = new List<KeyValuePair<string, string>>();
            filters.Add(new KeyValuePair<string, string>("PartitionKey", distributionSubmittalEntity.PartitionKey));
            filters.Add(new KeyValuePair<string, string>("ProcessStage", Enum.GetName(typeof(NotificationReceiverProcessStageEnum), NotificationReceiverProcessStageEnum.Created)));

            var notificationReceiverEntities = _tableStorage.GetTableEntitiesWithFilters<NotificationReceiverEntity>(filters);

            bool fristUtgaatt = DateTime.Now.Date > distributionSubmittalEntity.ReplyDeadline.Date;

            if (fristUtgaatt)
            {
                var entity = _tableStorage.GetTableEntityAsync<DistributionSubmittalEntity>(distributionSubmittalEntity.PartitionKey, distributionSubmittalEntity.PartitionKey).Result;
                entity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Reported);
                await _tableStorage.UpdateEntityRecordAsync<DistributionSubmittalEntity>(entity);

                return null;
            }
            else if (notificationReceiverEntities != null && notificationReceiverEntities.GetEnumerator().MoveNext())
            {
                SvarPaaVarselOmOppstartAvPlanarbeidModel svar = new SvarPaaVarselOmOppstartAvPlanarbeidModel();
                svar.InitialArchiveReference = distributionSubmittalEntity.PartitionKey;
                svar.Id = distributionSubmittalEntity.SenderId;
                svar.Type = AltinnReceiverType.Foretak;
                svar.Receivers = new List<SvarPaaVarselOmOppstartAvPlanarbeidReceiverModel>();
                
                foreach (var notificationReceiverEntity in notificationReceiverEntities)
                {
                    svar.PlanId = notificationReceiverEntity.PlanId;
                    svar.PlanNavn = notificationReceiverEntity.PlanNavn;

                    var receiver = new SvarPaaVarselOmOppstartAvPlanarbeidReceiverModel();
                    receiver.ReceiverName = notificationReceiverEntity.ReceiverName;
                    receiver.ReceiverPhone = notificationReceiverEntity.ReceiverPhone;
                    receiver.ReceiverEmail = notificationReceiverEntity.ReceiverEmail;
                    receiver.Reply = notificationReceiverEntity.Reply;
                    receiver.ReceiversArchiveReference = notificationReceiverEntity.RowKey;
                    svar.Receivers.Add(receiver);
                }
            
                return svar;
            }

            return null;
        }

        private async Task SendReceiptToSubmitterWhenSomeReceiversHaveRepliedAsync(SvarPaaVarselOmOppstartAvPlanarbeidModel answer, string publicContainer)
        {
            try
            {
                _log.LogDebug($"Start SendReceiptToSubmitterWhenSomReceiversHaveRepliedAsync for planId: {answer.PlanId}");

                DistributionSubmittalEntity submittalEntity = await _tableStorage.GetTableEntityAsync<DistributionSubmittalEntity>(answer.InitialArchiveReference, answer.InitialArchiveReference);
                submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.ReportingInProgress);
                _log.LogInformation($"{GetType().Name}. ArchiveReference={answer.InitialArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. Reporting in progress...");
                var notificationMessage = new AltinnNotificationMessage();
                notificationMessage.ArchiveReference = answer.InitialArchiveReference;
                notificationMessage.Receiver = new AltinnReceiver() { Id = answer.Id, Type = answer.Type };

                var reportHtml = GetReport(answer);
                _log.LogDebug("Start converting the attachment report to PDF");
                byte[] PDFInbytes = await _htmlToPdfConverterHttpClient.Get(reportHtml);
                _log.LogDebug("Converted to PDF");

                _log.LogDebug("Getting the Altinn report message");
                var messageData = GetReportMessage(answer, publicContainer);

                char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
                var validFilename = new string(answer.PlanNavn.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
                
                notificationMessage.MessageData = messageData;
                var reportAttachment = new AttachmentBinary()
                {
                    BinaryContent = PDFInbytes,
                    Filename = $"Uttalelser-{validFilename}-{DateTime.Now.ToString("dd. MMM yyyy, kl.HH.mm")}.pdf",
                    Name = $"Uttalelser pr. {DateTime.Now.ToString("dd MMM yyyy")}",
                    ArchiveReference = answer.InitialArchiveReference
                };
                
                notificationMessage.Attachments = new List<Attachment>() { reportAttachment };
                _log.LogInformation($"{GetType().Name}. ArchiveReference={answer.InitialArchiveReference}. Sending report (notification).");
                _log.LogDebug("Start SendNotification");
               
                IEnumerable<DistributionResult> result = _notificationAdapter.SendNotification(notificationMessage);

                var sendingFailed = result.Any(x => x.DistributionComponent.Equals(DistributionComponent.Correspondence)
                                                   && (
                                                          x.Step.Equals(DistributionStep.Failed)
                                                        || x.Step.Equals(DistributionStep.UnableToReachReceiver)
                                                        || x.Step.Equals(DistributionStep.UnkownErrorOccurred)
                                                        ));

                if (!sendingFailed)
                {
                    _log.LogDebug($"Successfully sent report of replies to submitter for archiveReference {answer.InitialArchiveReference}.");
                    var allReceiversForSubmittal = await _tableStorage.GetTableEntitiesAsync<NotificationReceiverEntity>(answer.InitialArchiveReference.ToLower());
                    var entitiesToUpdate = new List<NotificationReceiverEntity>();
                    foreach (var receiver in answer.Receivers)
                    {
                        var filters = new List<KeyValuePair<string, string>>();
                        filters.Add(new KeyValuePair<string, string>("PartitionKey", answer.InitialArchiveReference));
                        filters.Add(new KeyValuePair<string, string>("RowKey", receiver.ReceiversArchiveReference));

                        var notificationReceiverEntity = await _tableStorage.GetTableEntityAsync<NotificationReceiverEntity>(answer.InitialArchiveReference, receiver.ReceiversArchiveReference);

                        notificationReceiverEntity.ProcessStage = Enum.GetName(typeof(NotificationReceiverProcessStageEnum), NotificationReceiverProcessStageEnum.Reported);
                        entitiesToUpdate.Add(notificationReceiverEntity);
                    }

                    await _tableStorage.UpdateEntitiesAsync(entitiesToUpdate);

                    var tasks = entitiesToUpdate.Select(s => AddToReceiverProcessLogAsync(s.RowKey, s.ReceiverId, ReceiverStatusLogEnum.Completed));

                    _log.LogDebug("Start AddToReceiverProcessLogAsync");
                    await Task.WhenAll(tasks);
                    _log.LogDebug("End SendReceiptToSubmitterWhenSomeReceiversHaveRepliedAsync");
                }
                else
                {
                    _log.LogDebug($"Sending of report of replies to submitter for archiveReference {answer.InitialArchiveReference} failed.");
                    var failedStep = result.Where(x => x.Step.Equals(DistributionStep.Failed)
                                                        || x.Step.Equals(DistributionStep.UnableToReachReceiver)
                                                        || x.Step.Equals(DistributionStep.UnkownErrorOccurred)).Select(y => y.Step).First();
                    throw new SendNotificationException("Error: Failed during sending report of neighbours replies to submitter", failedStep);
                }

            }
            catch (SendNotificationException ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Text}: {ex.DistriutionStep}");
                throw ex;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred when creating and sending receipt");
                throw;
            }
        }

        protected virtual async Task AddToReceiverProcessLogAsync(string receiverPartitionKey, string receiverID, ReceiverStatusLogEnum statusEnum)
        {
            try
            {
                NotificationReceiverLogEntity receiverEntity = new NotificationReceiverLogEntity(receiverPartitionKey, $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", receiverID, statusEnum);
                await _tableStorage.InsertEntityRecordAsync<NotificationReceiverLogEntity>(receiverEntity);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error adding receiver record for ID={receiverPartitionKey} and receiverID {receiverID}");
                throw;
            }
        }

        protected MessageDataType GetReportMessage(SvarPaaVarselOmOppstartAvPlanarbeidModel answer, string publicContainer)
        {
            try
            {
                string archiveReference = answer.InitialArchiveReference;
                string planNavn = answer.PlanNavn;
                string planId = answer.PlanId;

                string htmlBody = _htmlUtils.GetHtmlFromTemplate("FtB_FormLogic.Notifications.NotificationFormLogic.SvarVarselOppstartPlanarbeidLogic.SvarVarselOppstartPlanarbeidReportMessageBody.html");

                htmlBody = htmlBody.Replace("<arkivReferanse />", archiveReference.ToUpper());
                htmlBody = htmlBody.Replace("<planId />", planId);
                htmlBody = htmlBody.Replace("<planNavn />", planNavn);

                var metadataList = new List<KeyValuePair<string, string>>();

                foreach (var receiver in answer.Receivers)
                {
                    metadataList.Add(new KeyValuePair<string, string>("ReceiversArchiveReference", receiver.ReceiversArchiveReference));

                }
                
                var urlToPublicAttachments = _blobOperations.GetBlobUrlsFromPublicStorageByMetadataAsync(publicContainer, metadataList).Result;

                StringBuilder urlListAsHtml = new StringBuilder();

                foreach (var attachmentInfo in urlToPublicAttachments)
                {
                    urlListAsHtml.Append($"<li><a href='{attachmentInfo.attachmentFileUrl}' target='_blank'>{attachmentInfo.attachmentFileName}</a></li>");
                }

                htmlBody = htmlBody.Replace("<svarbrevsliste />", urlListAsHtml.ToString());

                var mess = new MessageDataType()
                {
                    //TODO: Remove "AF-Ver.2: " and ({DateTime.Now.ToString("HH:mm:ss")}) from MessageTitle
                    MessageTitle = $"AF-Ver.2: Rapport - svar fra berørte parter ang. varsel for oppstart av planarbeid, {planNavn} ({DateTime.Now.ToString("HH:mm:ss")})",
                    MessageSummary = "Trykk på vedleggene under for å åpne rapporten eller svarene fra de berørte partene",
                    MessageBody = htmlBody
                };

                return mess;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while getting submitter receipt message");

                throw;
            }
        }

        protected string GetReport(SvarPaaVarselOmOppstartAvPlanarbeidModel answer)
        {
            try
            {
                string planNavn = answer.PlanNavn;
                string planId = answer.PlanId;
                string arkivReferanse = answer.InitialArchiveReference;
                var tableRowsAsHtml = string.Join("", answer.Receivers.Select(p => "<tr><td>" + p.ReceiverName + "</td><td>" + p.ReceiverPhone + "</td><td>" + p.ReceiverEmail + "</td></tr>"
                                                                                       + "<tr><td colspan='3'><label>Uttalelse</label></td></tr>"
                                                                                       + "<tr><td colspan='3'>" + p.Reply + "</td></tr>"
                                                                                       + "<tr><td colspan='3'><hr></td></tr>"));

                string htmlTemplate = _htmlUtils.GetHtmlFromTemplate("FtB_FormLogic.Notifications.NotificationFormLogic.SvarVarselOppstartPlanarbeidLogic.SvarVarselOppstartPlanarbeidReport.html");
                htmlTemplate = htmlTemplate.Replace("<planId />", planId);
                htmlTemplate = htmlTemplate.Replace("<planNavn />", planNavn);
                htmlTemplate = htmlTemplate.Replace("<arkivReferanse />", arkivReferanse.ToUpper());
                htmlTemplate = htmlTemplate.Replace("<svarFraBeroertPart />", tableRowsAsHtml);

                return htmlTemplate;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred when getting report with replies from neighbours");

                throw;
            }
        }
    }
}
