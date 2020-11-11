﻿using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Altinn2.Adapters.WS.Prefill;
using AltinnWebServices.WS.Prefill;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Altinn2.Adapters
{
    public class PrefillAdapter : IPrefillAdapter
    {
        private readonly ILogger _logger;
        private readonly IPrefillFormTaskBuilder _prefillFormTaskBuilder;
        private readonly IPrefillClient _altinnPrefillClient;

        public PrefillAdapter(ILogger<PrefillAdapter> logger, IPrefillFormTaskBuilder prefillFormTaskBuilder, IPrefillClient altinnPrefillClient)
        {
            _logger = logger;
            _prefillFormTaskBuilder = prefillFormTaskBuilder;
            _altinnPrefillClient = altinnPrefillClient;
        }

        public IEnumerable<PrefillResult> SendPrefill(AltinnDistributionMessage altinnDistributionMessage)
        {
            var results = new List<PrefillResult>();
            _logger.LogDebug($"{GetType().Name}: SendPrefill for receiver {altinnDistributionMessage.NotificationMessage.Receiver.Id}....");
            _prefillFormTaskBuilder.SetupPrefillFormTask(altinnDistributionMessage.PrefillServiceCode,
                    int.Parse(altinnDistributionMessage.PrefillServiceEditionCode),
                    altinnDistributionMessage.NotificationMessage.Receiver.Id,
                    altinnDistributionMessage.DistributionFormReferenceId.ToString(),
                    altinnDistributionMessage.DistributionFormReferenceId.ToString(),
                    altinnDistributionMessage.DistributionFormReferenceId.ToString(),
                    altinnDistributionMessage.DaysValid);

            _prefillFormTaskBuilder.AddPrefillForm(altinnDistributionMessage.PrefillDataFormatId,
                    int.Parse(altinnDistributionMessage.PrefillDataFormatVersion),
                    altinnDistributionMessage.PrefilledXmlDataString,
                    altinnDistributionMessage.DistributionFormReferenceId.ToString());

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
            results.Add(new PrefillResult() { Step = DistriutionStep.PayloadCreated, Message = $"{altinnDistributionMessage.NotificationMessage.Receiver.Id}" });
            _logger.LogDebug($"PrefillFormTask for {altinnDistributionMessage.NotificationMessage.Receiver.Id} - created");

            // ********** Should have retry for communication errors  *********

            ReceiptExternal receiptExternal = null;
            try
            {
                receiptExternal = _altinnPrefillClient.SendPrefill(prefillFormTask, altinnDistributionMessage.DueDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when sending prefill to Altinn");
                //throw;
            }
            // ****************************************************************


            var prefillFinalResult = new PrefillResult();

            if (receiptExternal?.ReceiptStatusCode == ReceiptStatusEnum.OK)
            {
                prefillFinalResult.Message = "Ok - Prefill sent";
                prefillFinalResult.Step = DistriutionStep.Sent;

                if (receiptExternal.References.Where(r => r.ReferenceTypeName == ReferenceType.WorkFlowReference).FirstOrDefault() != null)
                    prefillFinalResult = new PrefillSentResult() { PrefillReferenceId = receiptExternal.References.Where(r => r.ReferenceTypeName == ReferenceType.WorkFlowReference).First().ReferenceValue, Message = prefillFinalResult.Message, Step = prefillFinalResult.Step };
            }
            else if (receiptExternal?.ReceiptStatusCode != ReceiptStatusEnum.OK)
                if (receiptExternal.ReceiptText.Contains("Reportee is reserved against electronic communication"))
                {
                    prefillFinalResult.Message = receiptExternal.ReceiptText;
                    prefillFinalResult.Step = DistriutionStep.ReservedReportee;
                }
                else if (receiptExternal.ReceiptText.Contains("One or more notification endpoint addresses is missing.") ||
                         receiptExternal.ReceiptText.Contains("Unable to properly identify notification receivers") ||
                         receiptExternal.ReceiptText.Contains("no official (kofuvi) notification endpoints found"))
                {
                    prefillFinalResult.Message = receiptExternal.ReceiptText;
                    prefillFinalResult.Step = DistriutionStep.UnableToReachReceiver;
                }
                else
                {
                    prefillFinalResult.Message = "Unknown error occurred while sending prefill";
                    prefillFinalResult.Step = DistriutionStep.UnkownErrorOccurred;
                }
            else
            {
                prefillFinalResult.Message = "Unknown error occurred while sending prefill";
                prefillFinalResult.Step = DistriutionStep.UnkownErrorOccurred;
            }

            results.Add(prefillFinalResult);
            return results;
        }
    }
}