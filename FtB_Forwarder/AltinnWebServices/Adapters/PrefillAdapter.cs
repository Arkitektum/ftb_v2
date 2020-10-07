using AltinnWebServices.WS.Prefill;
using FtB_Common.Adapters;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AltinnWebServices.Services
{
    public class Altinn2PrefillAdapter : IPrefillAdapter
    {
        private readonly ILogger _logger;
        private readonly IPrefillFormTaskBuilder _prefillFormTaskBuilder;
        private readonly IAltinnPrefillClient _altinnPrefillClient;

        public Altinn2PrefillAdapter(ILogger<Altinn2PrefillAdapter> logger, IPrefillFormTaskBuilder prefillFormTaskBuilder, IAltinnPrefillClient altinnPrefillClient)
        {
            _logger = logger;
            _prefillFormTaskBuilder = prefillFormTaskBuilder;
            _altinnPrefillClient = altinnPrefillClient;
        }

        public PrefillResult SendPrefill(PrefillData prefillData)
        {
            _prefillFormTaskBuilder.SetupPrefillFormTask(prefillData.ServiceCode, int.Parse(prefillData.ServiceEditionCode), prefillData.Reciever, prefillData.DistributionFormId, prefillData.DistributionFormId, prefillData.DistributionFormId, prefillData.DaysValid);
            _prefillFormTaskBuilder.AddPrefillForm(prefillData.DataFormatId, int.Parse(prefillData.DataFormatVersion), prefillData.XmlDataString, prefillData.DistributionFormId);

            //Map email thingy!!!!
            //if (prefillFormData.DoEmailNotification())
            //{
            //    if (prefillFormData.GetNotificationChannel() == NotificationEnums.NotificationChannel.Prefill)
            //    {
            //        var enotification = prefillFormData.GetEmailNotification();
            //        var notificationTemplate = prefillFormData.GetAltinnNotificationTemplate();
            //        string emailcontent = MessageFormatter.ReplaceStaticArchiveReferenceWithActual(enotification.EmailContent, archivereferance);
            //        prefillFormTaskBuilder.AddEmailAndSmsNotification(Resources.TextStrings.DistributionFromEmail, enotification.Email, enotification.EmailSubject, emailcontent, notificationTemplate, enotification.SmsContent);
            //    }

            //    if (prefillFormData.GetNotificationChannel() == NotificationEnums.NotificationChannel.CorrespondenceWithPrefillEndpointValidation)
            //    {
            //        prefillFormTaskBuilder.ValidateNotificationEndpointsWithPrefillInstantiation();
            //    }
            //}
            var prefillFormTask = _prefillFormTaskBuilder.Build();
            _logger.LogDebug($"PrefillFormTask for {prefillData.Reciever} - created");

            // ********** Should have retry for communication errors  *********

            ReceiptExternal receiptExternal = null;
            try
            {
                receiptExternal = _altinnPrefillClient.SendPrefill(prefillFormTask, prefillData.DueDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when sending prefill to Altinn");
                throw;
            }
            // ****************************************************************


            var prefillResult = new PrefillResult();

            if (receiptExternal?.ReceiptStatusCode == ReceiptStatusEnum.OK)
            {
                prefillResult.ResultMessage = "Ok - Prefill sent";
                prefillResult.ResultType = PrefillResultType.Ok;

                if (receiptExternal.References.Where(r => r.ReferenceTypeName == ReferenceType.WorkFlowReference).FirstOrDefault() != null)
                    prefillResult.PrefillReferenceId = receiptExternal.References.Where(r => r.ReferenceTypeName == ReferenceType.WorkFlowReference).First().ReferenceValue;
            }
            else if (receiptExternal?.ReceiptStatusCode != ReceiptStatusEnum.OK)
                if (receiptExternal.ReceiptText.Contains("Reportee is reserved against electronic communication"))
                {
                    prefillResult.ResultMessage = receiptExternal.ReceiptText;
                    prefillResult.ResultType = PrefillResultType.ReservedReportee;
                }
                else if (receiptExternal.ReceiptText.Contains("One or more notification endpoint addresses is missing.") ||
                         receiptExternal.ReceiptText.Contains("Unable to properly identify notification receivers") ||
                         receiptExternal.ReceiptText.Contains("no official (kofuvi) notification endpoints found"))
                {
                    prefillResult.ResultMessage = receiptExternal.ReceiptText;
                    prefillResult.ResultType = PrefillResultType.UnableToReachReceiver;
                }
                else
                {
                    prefillResult.ResultMessage = "Unknown error occurred while sending prefill";
                    prefillResult.ResultType = PrefillResultType.UnkownErrorOccured;
                }
            else
            {
                prefillResult.ResultMessage = "Unknown error occurred while sending prefill";
                prefillResult.ResultType = PrefillResultType.UnkownErrorOccured;
            }

            return prefillResult;
        }
    }
}