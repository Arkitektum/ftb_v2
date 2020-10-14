using Altinn.Common.Models;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using FtB_DataModels.Mappers;
using no.kxml.skjema.dibk.nabovarselPlan;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidPrepareAltinn3SendDataProvider : SendDataProviderBase, IDistributionDataMapper<FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType, no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        //public VarselOppstartPlanarbeidPrepareAltinn3SendDataProvider(IDecryptionFactory decryptionFactory) : base(decryptionFactory)
        //{
        //}

        public FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType PrefillFormData { get; set; }

        public AltinnDistributionMessage GetDistributionMessage(string prefillXmlString, NabovarselPlanType mainFormData, string distributionFormId, string archiveReference)
        {
            PrefillFormData = SerializeUtil.DeserializeFromString<FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType>(prefillXmlString);

            var prefillData = new AltinnDistributionMessage()
            {
                PrefillDataFormatId = PrefillFormData.dataFormatId,
                PrefillDataFormatVersion = PrefillFormData.dataFormatVersion,
                //Receiver = base.GetReceiver(NabovarselPlanAltinn3Mappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
                DistributionFormReferenceId = distributionFormId,
                PrefillServiceCode = "5419",
                PrefillServiceEditionCode = "1",
                PrefilledXmlDataString = prefillXmlString,
                DaysValid = 14,
                DueDate = null
            };

            prefillData.NotificationMessage = new AltinnNotificationMessage()
            {
                Receiver = base.GetReceiver(NabovarselPlanAltinn3Mappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
                ArchiveReference = archiveReference
            };

            return prefillData;
        }
    }
}
