using AltinnWebServices.WS.Prefill;
using FtB_Common.Adapters;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace AltinnWebServices.Services
{
    public class PrefillAdapter : IPrefillAdapter
    {
        private readonly IPrefillFormTaskBuilder _prefillFormTaskBuilder;
        private readonly IAltinnPrefillClient _altinnPrefillClient;
        private readonly ILogger<PrefillAdapter> _log;

        public PrefillAdapter(IPrefillFormTaskBuilder prefillFormTaskBuilder, IAltinnPrefillClient altinnPrefillClient, ILogger<PrefillAdapter> log)
        {
            _prefillFormTaskBuilder = prefillFormTaskBuilder;
            _altinnPrefillClient = altinnPrefillClient;
            _log = log;
        }

        public PrefillResult SendPrefill(PrefillData prefillData)
        {
            _log.LogDebug($"{GetType().Name}: SendPrefill for receiver {prefillData.Reciever}....");
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


            // ********** Should have retry for communication errors  *********
            var receiptExternal = _altinnPrefillClient.SendPrefill(prefillFormTask, prefillData.DueDate);
            // ****************************************************************


            var prefillResult = new PrefillResult();

            if (receiptExternal?.ReceiptStatusCode == ReceiptStatusEnum.OK)
            {
                prefillResult.ResultMessage = "Ok - Prefill sent";
                prefillResult.ResultType = PrefillResultType.Ok;
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