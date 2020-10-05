using FtB_Common.Adapters;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;using FtB_Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ProcessStrategies
{
    public class DefaultDistributionSendStrategy : SendStrategyBase
    {
        private readonly IPrefillAdapter _prefillAdapter;
        private readonly ILogger<DefaultDistributionSendStrategy> _log;

        public DefaultDistributionSendStrategy(IFormDataRepo repo, ITableStorage tableStorage, IPrefillAdapter prefillAdapter
                                                , ILogger<DefaultDistributionSendStrategy> log) : base(repo, tableStorage, log)
        {
            _prefillAdapter = prefillAdapter;
            _log = log;
        }

        public override ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            try
            {
                _log.LogDebug($"{GetType().Name}: Execute for {FormLogicBeingProcessed.ArchiveReference}");

                PrefillData prefillData;
                //Act upon RECEIVER ENTITY STATUS - Find some better way of orchestrating this than what's below....
                /*
                switch: receiverEntityStatus from TableStorage
                case: Created
                    CreatePrefillXML(sendQueueItem);
                    prefillData = PersistPrefill(sendQueueItem);
                    SendPrefill(sendQueueItem, prefillData);
                    Distribute(sendQueueItem);

                case: PrefillCreated
                    prefillData = PersistPrefill(sendQueueItem);
                    SendPrefill(sendQueueItem, prefillData);
                    Distribute(sendQueueItem);

                case: PrefillPersisted
                    SendPrefill(sendQueueItem, prefillData);
                    Distribute(sendQueueItem);

                case: PrefillSent
                    Distribute(sendQueueItem);

                case: Sent
                    Nothing
                */



                CreatePrefillXML(sendQueueItem);

                prefillData = PersistPrefill(sendQueueItem);

                SendPrefill(sendQueueItem, prefillData);

                Distribute(sendQueueItem);


                return base.Exceute(sendQueueItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Distribute(SendQueueItem sendQueueItem)
        {
            // Validate if receiver info is sufficient

            // Decrypt

            // Create distributionform 


            // Map  from prefill-data to prefillFormTask
            // Send using prefill service

            // _distributionAdapter.SendDistribution(eitEllerAnnaObjekt);

            // Finally persist distributionform..  and maybe a list of logentries??            

            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.Sent));
        }

        private void SendPrefill(SendQueueItem sendQueueItem, PrefillData prefillData)
        {
            var prefillResult = _prefillAdapter.SendPrefill(prefillData);// .SendPrefill(FormLogicBeingProcessed.ArchiveReference, sendQueueItem.Receiver.Id);
            switch (prefillResult.ResultType)
            {
                case PrefillResultType.Ok:
                    UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillSent));
                    break;
                case PrefillResultType.UnkownErrorOccured:
                    break;
                case PrefillResultType.ReservedReportee:
                    break;
                case PrefillResultType.UnableToReachReceiver:
                    break;
                default:
                    break;
            }
        }

        private PrefillData PersistPrefill(SendQueueItem sendQueueItem)
        {
            var prefillData = GeneratePrefillData(sendQueueItem.Receiver.Id);
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
            repo.AddBytesAsBlob(FormLogicBeingProcessed.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(prefillData.XmlDataString), metaData);
            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillPersisted));

            return prefillData;
        }

        private void CreatePrefillXML(SendQueueItem sendQueueItem)
        {
            // Get prefill data generated from formlogic
            // Map to a specific type i.e. Prefill-type for altinn
            FormLogicBeingProcessed.ProcessSendStep(sendQueueItem.Receiver.Id);
            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillCreated));
        }
        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }

        protected virtual PrefillData GeneratePrefillData(string receiverId)
        {
            var distributionIdentifier = Guid.NewGuid();
            return FormLogicBeingProcessed.GetPrefillData(receiverId, distributionIdentifier.ToString());
        }


        //public void SendPrefillForm(FormData distributionServiceFormData, IPrefillForm prefillFormData, string archivereferance, IAltinnForm altinnForm)
        //{
        //    try
        //    {
        //        string decryptedReportee;
        //        string presentationReportee;
        //        WS.AltinnPreFill.ReceiptExternal receiptExternal = null;

        //        DistributionForm dForm = _formMetadataService.InsertDistributionForm(archivereferance, prefillFormData.GetPrefillKey(), prefillFormData.GetPrefillOurReference(), altinnForm.GetName());
        //        //  _logEntryService.Save(new LogEntry(archivereferance, $"Starter distribusjon med søknadsystemsreferanse {prefillFormData.GetPrefillOurReference()}", LogEntry.Info, LogEntry.ExternalMsg));

        //        // Add "dummy" distributions for combined nabovarsel distributions
        //        // ToojDo: Make this a seperate function very soon
        //        if (altinnForm is INabovarselSvar)
        //        {
        //            var neighborReply = (INabovarselSvar)altinnForm;
        //            var neighborReferenceIdList = neighborReply.GetSluttbrukersystemVaarReferanse();
        //            if (neighborReferenceIdList != null && neighborReferenceIdList.Count > 1)
        //            {
        //                // Add reference ID to the first distribution
        //                dForm.DistributionReference = dForm.Id;
        //                _formMetadataService.SaveDistributionForm(dForm);

        //                for (int i = 1; i < neighborReferenceIdList.Count; i++)
        //                {
        //                    DistributionForm dFormDummy = _formMetadataService.InsertDistributionForm(archivereferance, prefillFormData.GetPrefillKey(), neighborReferenceIdList[i], altinnForm.GetName());
        //                    dFormDummy.DistributionReference = dForm.Id;
        //                    //_formMetadataService.SaveDistributionForm(dFormDummy);
        //                    //_logEntryService.Save(new LogEntry(archivereferance, $"Distribusjon med søknadsystemsreferanse {prefillFormData.GetPrefillOurReference()} kombinert med {neighborReferenceIdList[i]}", LogEntry.Info, LogEntry.ExternalMsg));
        //                    //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} kombinert med {dFormDummy.Id.ToString()}", "Info", true));
        //                }
        //            }
        //        }

        //        prefillFormData.SetPrefillKey(dForm.Id.ToString());
        //        //  _logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Distribusjon av {altinnForm.GetName()} til tjeneste {prefillFormData.GetPrefillServiceCode()}/{prefillFormData.GetPrefillServiceEditionCode()}", "Info", true));


        //        if (String.IsNullOrEmpty(prefillFormData.GetPrefillSendToReporteeId()))
        //        {
        //            //_logEntryService.Save(new LogEntry(archivereferance, "Fant ikke personnummer/organisasjonsnummer", LogEntry.Error, LogEntry.ExternalMsg));
        //            //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Fant ikke personnummer/organisasjonsnummer i prefillFormData.GetPrefillSendToReporteeId()", LogEntry.Error, LogEntry.InternalMsg));

        //            dForm.DistributionStatus = DistributionStatus.error;
        //            _formMetadataService.SaveDistributionForm(dForm);
        //            _syncRecordsOfCombinedDistributions.Sync(altinnForm, dForm);

        //            // Returning out of distribution loop to process the next entry 
        //            return;
        //        }

        //        if (prefillFormData.GetPrefillSendToReporteeId().Length > 11)
        //        {
        //            //  _logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Dekrypterer fødselsnummer", LogEntry.Info, LogEntry.InternalMsg));
        //            decryptedReportee = _decryptionFactory.GetDecryptor().DecryptText(prefillFormData.GetPrefillSendToReporteeId());
        //            presentationReportee = "kryptert personnummer";
        //        }
        //        else
        //        {
        //            decryptedReportee = prefillFormData.GetPrefillSendToReporteeId();
        //            presentationReportee = decryptedReportee;
        //        }


        //        PrefillFormTaskBuilder prefillFormTaskBuilder = new PrefillFormTaskBuilder();

        //        prefillFormTaskBuilder.SetupPrefillFormTask(prefillFormData.GetPrefillServiceCode(), Convert.ToInt32(prefillFormData.GetPrefillServiceEditionCode()), decryptedReportee, dForm.Id.ToString(), dForm.Id.ToString(), dForm.Id.ToString(), 14);
        //        prefillFormTaskBuilder.AddPrefillForm(altinnForm.GetDataFormatId(), Convert.ToInt32(altinnForm.GetDataFormatVersion()), prefillFormData.GetFormXML(), dForm.Id.ToString());


        //        if (prefillFormData.DoEmailNotification())
        //        {
        //            if (prefillFormData.GetNotificationChannel() == NotificationEnums.NotificationChannel.Prefill)
        //            {
        //                var enotification = prefillFormData.GetEmailNotification();
        //                var notificationTemplate = prefillFormData.GetAltinnNotificationTemplate();
        //                string emailcontent = MessageFormatter.ReplaceStaticArchiveReferenceWithActual(enotification.EmailContent, archivereferance);
        //                prefillFormTaskBuilder.AddEmailAndSmsNotification(Resources.TextStrings.DistributionFromEmail, enotification.Email, enotification.EmailSubject, emailcontent, notificationTemplate, enotification.SmsContent);
        //            }

        //            if (prefillFormData.GetNotificationChannel() == NotificationEnums.NotificationChannel.CorrespondenceWithPrefillEndpointValidation)
        //            {
        //                prefillFormTaskBuilder.ValidateNotificationEndpointsWithPrefillInstantiation();
        //            }
        //        }




        //        PrefillFormTask preFillData = prefillFormTaskBuilder.Build();
        //        DateTime? dueDate = null;
        //        if (prefillFormData.GetPrefillNotificationDueDays() > 0)
        //            dueDate = DateTime.Now.AddDays(prefillFormData.GetPrefillNotificationDueDays());

        //        try
        //        {
        //            //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Sender prefill kall til Altinn", LogEntry.Info, true));
        //            receiptExternal = _altinnPrefillService.SubmitAndInstantiatePrefilledForm(preFillData, dueDate);

        //        }
        //        catch (Exception e)
        //        {
        //            //_log.Error(e, "An error occurred when submitting prefill form {prefillKey}", prefillFormData.GetPrefillKey());
        //            receiptExternal = null;
        //            string innerExcpetion = "";
        //            if (e.InnerException != null)
        //                innerExcpetion = e.InnerException.Message;
        //            //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - unntak i WS Prefill: {e.Message}, {innerExcpetion}", LogEntry.Error, true));
        //        }

        //        if (receiptExternal == null)
        //        {
        //            // _logEntryService.Save(new LogEntry(archivereferance, $"Unntak ved Altinn utsendelse av {altinnForm.GetName()} til {presentationReportee}", LogEntry.Error, LogEntry.ExternalMsg));
        //            //LogEntry logSsnOrg = LogEntry.NewErrorInternal(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Unntak ved Altinn prefill av {altinnForm.GetName()} til >>>{decryptedReportee}<<<", ProcessLabel.AltinnDistributionPostProcess, stopWatch);
        //            //_logEntryService.Save(logSsnOrg);
        //            _formMetadataService.SaveDistributionFormStatusSubmittedPrefilledError(dForm.Id, DateTime.Now, "Unntak i Altinn prefill Web Service utsendelse");
        //            _syncRecordsOfCombinedDistributions.Sync(altinnForm, dForm);
        //        }
        //        else if (receiptExternal.ReceiptStatusCode != WS.AltinnPreFill.ReceiptStatusEnum.OK)
        //        {
        //            //if reportee isReservable --> todo - støttes ikke i Altinn Prefill
        //            if (distributionServiceFormData.Mainform is INabovarselDistribution && receiptExternal.ReceiptText.Contains("Reportee is reserved against electronic communication"))
        //            {
        //                // _logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Nabovarsel print pga. reservasjon", LogEntry.Info, LogEntry.InternalMsg));
        //                SendNabovarselThroughPrintAndMailService(distributionServiceFormData, prefillFormData, archivereferance, altinnForm, presentationReportee, decryptedReportee, dForm);
        //            }
        //            else if (distributionServiceFormData.Mainform is INabovarselDistribution &&
        //                     (
        //                         receiptExternal.ReceiptText.Contains("One or more notification endpoint addresses is missing.") ||
        //                         receiptExternal.ReceiptText.Contains("Unable to properly identify notification receivers") ||
        //                         receiptExternal.ReceiptText.Contains("no official (kofuvi) notification endpoints found")
        //                     ))
        //            {
        //                // :  <a:ReceiptText>One or more notification endpoint addresses is missing. Altinn does not have an address to fill the missing address Reportee: 01024100991</a:ReceiptText> 
        //                //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Nabovarsel print pga. manglende endepunkter", LogEntry.Info, LogEntry.InternalMsg));
        //                SendNabovarselThroughPrintAndMailService(distributionServiceFormData, prefillFormData, archivereferance, altinnForm, presentationReportee, decryptedReportee, dForm);
        //            }
        //            else
        //            {
        //                //_logEntryService.Save(new LogEntry(archivereferance, $"Feil ved Altinn utsendelse av {altinnForm.GetName()} til {presentationReportee}"));
        //                //LogEntry logSsnOrg = LogEntry.NewErrorInternal(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Feil ved Altinn prefill til >>>{decryptedReportee}<<<: {receiptExternal.ReceiptStatusCode} {receiptExternal.ReceiptText}", ProcessLabel.AltinnDistributionPostProcess, stopWatch);
        //                //_logEntryService.Save(logSsnOrg);

        //                _formMetadataService.SaveDistributionFormStatusSubmittedPrefilledError(dForm.Id, receiptExternal.LastChanged, receiptExternal.ReceiptText);
        //                _syncRecordsOfCombinedDistributions.Sync(altinnForm, dForm);
        //            }
        //        }
        //        else
        //        {

        //            _formMetadataService.SaveDistributionFormStatusSubmittedPrefilled(dForm.Id, DistributionStatus.submittedPrefilled, receiptExternal.ReceiptId.ToString(), receiptExternal.LastChanged);
        //            _syncRecordsOfCombinedDistributions.Sync(altinnForm, dForm);
        //            // _logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Klar til å sende correspondence", LogEntry.Info, LogEntry.InternalMsg));

        //            string workflowReferenceId = "";
        //            if (receiptExternal.References.Where(r => r.ReferenceTypeName == WS.AltinnPreFill.ReferenceType.WorkFlowReference).FirstOrDefault() != null) workflowReferenceId = receiptExternal.References.Where(r => r.ReferenceTypeName == WS.AltinnPreFill.ReferenceType.WorkFlowReference).First().ReferenceValue;

        //            //LogEntry logEntry;
        //            if (SendNotification(distributionServiceFormData, decryptedReportee, presentationReportee, prefillFormData, dForm.Id, workflowReferenceId, distributionServiceFormData.ArchiveReference, CheckDueDate(altinnForm)))
        //            {
        //                //logEntry = LogEntry.NewInfo(archivereferance, $"Altinn {altinnForm.GetName()} laget til ({presentationReportee}), Altinn kvitteringsid {receiptExternal.ReceiptId}", ProcessLabel.AltinnSubmitPrefill, stopWatch);
        //            }
        //            else
        //            {
        //                //logEntry = LogEntry.NewInfo(archivereferance, $"Altinn {altinnForm.GetName()} pefill OK men correspondence feilet til ({presentationReportee}), Altinn prefill kvitteringsid {receiptExternal.ReceiptId}", ProcessLabel.AltinnSubmitPrefill, stopWatch);
        //            }
        //            //_logEntryService.Save(logEntry);
        //            //_log.Information(logEntry.ToString());

        //            //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Distribusjon behandling ferdig", LogEntry.Info, LogEntry.InternalMsg));
        //        }
        //    }
        //    catch (FaultException ex)
        //    {
        //        //var logEntry = LogEntry.NewError(archivereferance, $"Feil ved Altinn preutfylling av {altinnForm.GetName()} " + ex.Message, ProcessLabel.AltinnSubmitPrefill, stopWatch);
        //        //_logEntryService.Save(logEntry);
        //        //_log.Error(ex, logEntry.ToString());

        //        //_logEntryService.Save(new LogEntry(archivereferance, $"Dist id {prefillFormData.GetPrefillKey()} - Generellt unntak i Prefill: {ex.Message}", LogEntry.Error, LogEntry.InternalMsg));
        //    }
        //}
    }
}
