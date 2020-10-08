using FtB_Common.Interfaces;
using FtB_Common.Utils;
using FtB_DataModels.Mappers;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidPrepareAltinn3PrefillDataProvider : PrefillDataProviderBase, IPrefillDataProvider<FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType>
    {
        public FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType PrefillFormData { get; set; }

        public PrefillData GetPrefillData(string xmlString, string distributionFormId)
        {            
            PrefillFormData = SerializeUtil.DeserializeFromString<FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType>(xmlString);

            var prefillData = new PrefillData()
            {
                DataFormatId = PrefillFormData.dataFormatId,
                DataFormatVersion = PrefillFormData.dataFormatVersion,
                Reciever = base.GetReceiver(NabovarselPlanAltinn3Mappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
                DistributionFormId = distributionFormId,
                ServiceCode = "5419",
                ServiceEditionCode = "1",
                XmlDataString = xmlString,
                DaysValid = 14,
                DueDate = null
            };

            return prefillData;
        }
    }
}
