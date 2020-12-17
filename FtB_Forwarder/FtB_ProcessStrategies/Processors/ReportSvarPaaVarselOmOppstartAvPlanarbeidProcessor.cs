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
                var answerToDistributionSubmitter = await GetNotificationSenderReplies(distributionSubmittalEntity);
                _log.LogDebug($"Number of answers for {distributionSubmittalEntity.PartitionKey} found: {((answerToDistributionSubmitter == null) || (answerToDistributionSubmitter.Senders.Count() == 0) ? "0" : answerToDistributionSubmitter.Senders.Count().ToString())}");

                if (answerToDistributionSubmitter != null)
                {
                    var publicBlobContainer = _blobOperations.GetPublicBlobContainerName(answerToDistributionSubmitter.InitialArchiveReference.ToLower());
                    _log.LogDebug($"Reporting PDF replies to submitter for archiveReference {answerToDistributionSubmitter.InitialArchiveReference }");
                    await SendRepliesReportToDistributionSubmitterAsync(answerToDistributionSubmitter, publicBlobContainer);
                }
            }
        }

        private async Task<IEnumerable<DistributionSubmittalEntity>> GetDistributionSubmittalEntities()
        {
            var distributionSubmittalEntitiesCompleted = _tableStorage.GetTableEntitiesWithStatusFilter<DistributionSubmittalEntity>(Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Completed));
            var distributionSubmittalEntitiesReportingInProgress = _tableStorage.GetTableEntitiesWithStatusFilter<DistributionSubmittalEntity>(Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.ReportingInProgress));

            IEnumerable<DistributionSubmittalEntity> distributionSubmittalEntities = distributionSubmittalEntitiesCompleted.Select(x => x).Concat(distributionSubmittalEntitiesReportingInProgress.Select(y => y));
            return distributionSubmittalEntities;
        }

        private async Task<SvarPaaVarselOmOppstartAvPlanarbeidModel> GetNotificationSenderReplies(DistributionSubmittalEntity distributionSubmittalEntity)
        {
            var filters = new List<KeyValuePair<string, string>>();
            filters.Add(new KeyValuePair<string, string>("PartitionKey", distributionSubmittalEntity.PartitionKey));
            filters.Add(new KeyValuePair<string, string>("ProcessStage", Enum.GetName(typeof(NotificationSenderProcessStageEnum), NotificationSenderProcessStageEnum.Created)));

            var notificationSendersReadyToReport = _tableStorage.GetTableEntitiesWithFilters<NotificationSenderEntity>(filters);

            bool fristUtgaatt = DateTime.Now.Date > distributionSubmittalEntity.ReplyDeadline.Date;

            if (fristUtgaatt)
            {
                var entity = _tableStorage.GetTableEntityAsync<DistributionSubmittalEntity>(distributionSubmittalEntity.PartitionKey, distributionSubmittalEntity.PartitionKey).Result;
                entity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.Reported);
                await _tableStorage.UpdateEntityRecordAsync<DistributionSubmittalEntity>(entity);

                return null;
            }
            else if (notificationSendersReadyToReport != null && notificationSendersReadyToReport.GetEnumerator().MoveNext())
            {
                SvarPaaVarselOmOppstartAvPlanarbeidModel reportData = new SvarPaaVarselOmOppstartAvPlanarbeidModel();
                reportData.InitialArchiveReference = distributionSubmittalEntity.PartitionKey;
                reportData.FristForInnspill = distributionSubmittalEntity.ReplyDeadline;
                reportData.Id = distributionSubmittalEntity.SenderId;
                reportData.AltinnReceiverType = AltinnReceiverType.Foretak;
                reportData.Senders = new List<SvarPaaVarselOmOppstartAvPlanarbeidSenderModel>();
                
                foreach (var notificationSender in notificationSendersReadyToReport)
                {
                    reportData.PlanId = notificationSender.PlanId;
                    reportData.PlanNavn = notificationSender.PlanNavn;

                    var sender = new SvarPaaVarselOmOppstartAvPlanarbeidSenderModel();
                    sender.SenderName = notificationSender.SenderName;
                    sender.SenderPhone = notificationSender.SenderPhone;
                    sender.SenderEmail = notificationSender.SenderEmail;
                    sender.Reply = notificationSender.Reply;
                    sender.SendersArchiveReference = notificationSender.RowKey;
                    reportData.Senders.Add(sender);
                }
            
                return reportData;
            }

            return null;
        }

        private async Task SendRepliesReportToDistributionSubmitterAsync(SvarPaaVarselOmOppstartAvPlanarbeidModel answer, string publicContainer)
        {
            try
            {
                _log.LogDebug($"Start SendRepliesReportToDistributionSubmitterAsync for planId: {answer.PlanId}");

                DistributionSubmittalEntity submittalEntity = await _tableStorage.GetTableEntityAsync<DistributionSubmittalEntity>(answer.InitialArchiveReference, answer.InitialArchiveReference);
                submittalEntity.Status = Enum.GetName(typeof(SubmittalStatusEnum), SubmittalStatusEnum.ReportingInProgress);
                _log.LogInformation($"{GetType().Name}. ArchiveReference={answer.InitialArchiveReference}.  SubmittalStatus: {submittalEntity.Status}. Reporting in progress...");
                var notificationMessage = new AltinnNotificationMessage();
                notificationMessage.ArchiveReference = answer.InitialArchiveReference;
                notificationMessage.Receiver = new AltinnReceiver() { Id = answer.Id, Type = answer.AltinnReceiverType };

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
                    Filename = $"Uttalelser_{validFilename}_{DateTime.Now.ToString("dd.MM.yyyy, kl.HH.mm")}.pdf",
                    Name = $"Uttalelser pr. {DateTime.Now.ToString("dd.MM.yyyy")}",
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
                    var entitiesToUpdate = new List<NotificationSenderEntity>();
                    foreach (var sender in answer.Senders)
                    {
                        var filters = new List<KeyValuePair<string, string>>();
                        filters.Add(new KeyValuePair<string, string>("PartitionKey", answer.InitialArchiveReference));
                        filters.Add(new KeyValuePair<string, string>("RowKey", sender.SendersArchiveReference));

                        var notificationSenderEntity = await _tableStorage.GetTableEntityAsync<NotificationSenderEntity>(answer.InitialArchiveReference, sender.SendersArchiveReference);

                        notificationSenderEntity.ProcessStage = Enum.GetName(typeof(NotificationSenderProcessStageEnum), NotificationSenderProcessStageEnum.Reported);
                        entitiesToUpdate.Add(notificationSenderEntity);
                    }

                    await _tableStorage.UpdateEntitiesAsync(entitiesToUpdate);

                    var tasks = entitiesToUpdate.Select(s => AddToSenderProcessLogAsync(s.RowKey, s.SenderId, NotificationSenderStatusLogEnum.Completed)); //Completed

                    _log.LogDebug("Start AddToSenderProcessLogAsync");
                    await Task.WhenAll(tasks);
                    _log.LogDebug("End SendRepliesReportToDistributionSubmitterAsync");
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

        protected virtual async Task AddToSenderProcessLogAsync(string initialArchiveReference, string senderId, NotificationSenderStatusLogEnum statusEnum)
        {
            try
            {
                NotificationSenderLogEntity senderEntity = new NotificationSenderLogEntity(initialArchiveReference, $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}", senderId, statusEnum);
                await _tableStorage.InsertEntityRecordAsync<NotificationSenderLogEntity>(senderEntity);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Error adding sender record for ID={initialArchiveReference} and senderId {senderId}");
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

                var fristForInnspill = answer.FristForInnspill;
                htmlBody = htmlBody.Replace("<fristForInnspill />", fristForInnspill.ToString("dd.MM.yyyy"));
                htmlBody = htmlBody.Replace("<arkivReferanse />", archiveReference.ToUpper());
                htmlBody = htmlBody.Replace("<planId />", planId);
                htmlBody = htmlBody.Replace("<planNavn />", planNavn);

                var metadataList = new List<KeyValuePair<string, string>>();

                foreach (var sender in answer.Senders)
                {
                    metadataList.Add(new KeyValuePair<string, string>("SendersArchiveReference", sender.SendersArchiveReference));

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
                string arkivReferanse = answer.InitialArchiveReference;
                var tableRowsAsHtml = string.Join("", answer.Senders.Select(p => "<tr><td>" + p.SenderName + "</td><td>" + p.SenderPhone + "</td><td>" + p.SenderEmail + "</td></tr>"
                                                                                       + "<tr><td colspan='3'><label>Uttalelse</label></td></tr>"
                                                                                       + "<tr><td colspan='3'>" + p.Reply + "</td></tr>"
                                                                                       + "<tr><td colspan='3'><hr></td></tr>"));

                string htmlTemplate = _htmlUtils.GetHtmlFromTemplate("FtB_FormLogic.Notifications.NotificationFormLogic.SvarVarselOppstartPlanarbeidLogic.SvarVarselOppstartPlanarbeidReport.html");
                htmlTemplate = htmlTemplate.Replace("<planId />", answer.PlanId);
                htmlTemplate = htmlTemplate.Replace("<planNavn />", answer.PlanNavn);
                htmlTemplate = htmlTemplate.Replace("<arkivReferanse />", arkivReferanse.ToUpper());
                htmlTemplate = htmlTemplate.Replace("<fristForInnspill />", answer.FristForInnspill.ToString("dd.MM.yyyy"));
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
