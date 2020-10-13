using Altinn.Common.Models;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using FtB_DataModels.Mappers;
using System;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidSendDataProvider : SendDataProviderBase, IDistributionDataMapper<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType, no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        //public VarselOppstartPlanarbeidSendDataProvider(IDecryptionFactory decryptionFactory) : base(decryptionFactory)
        //{
        //}

        public no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType PrefillFormData { get; set; }

        public AltinnDistributionMessage GetDistributionMessage(string prefillXmlString, no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType mainFormData, string distributionFormId)
        {
            PrefillFormData = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>(prefillXmlString);

            var distributionMessage = new AltinnDistributionMessage()
            {
                PrefillDataFormatId = PrefillFormData.dataFormatId,
                PrefillDataFormatVersion = PrefillFormData.dataFormatVersion,
                DistributionFormReferenceId = distributionFormId,
                PrefillServiceCode = "5419", //Distribution service servicecode??
                PrefillServiceEditionCode = "1",
                PrefilledXmlDataString = prefillXmlString,
                DaysValid = 14,
                DueDate = null
            };

            distributionMessage.NotificationMessage.Receiver = base.GetReceiver(NabovarselPlanMappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart));
            distributionMessage.NotificationMessage.MessageData = CreateMessageData(mainFormData);
            return distributionMessage;
        }

        private MessageDataType CreateMessageData(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType mainFormData)
        {
            //return new MessageDataType((values, messageBody) =>
            //{
            //    foreach (var item in values)
            //    {
            //        messageBody.Replace(item.Key, item.Value);
            //    }
            //})
            return new MessageDataType() { MessageBody = "", MessageSummary = "", MessageTitle = "" };
        }

        //public PrefillData GetPrefillData(string xmlString, string distributionFormId)
        //{            
        //    PrefillFormData = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>(xmlString);

        //    var prefillData = new PrefillData()
        //    {
        //        DataFormatId = PrefillFormData.dataFormatId,
        //        DataFormatVersion = PrefillFormData.dataFormatVersion,
        //        Receiver = base.GetReceiver(NabovarselPlanMappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
        //        DistributionFormId = distributionFormId,
        //        ServiceCode = "5419",
        //        ServiceEditionCode = "1",
        //        XmlDataString = xmlString,
        //        DaysValid = 14,
        //        DueDate = null
        //    };

        //    return prefillData;
        //}
    }
}
