using Altinn.Common.Models;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using FtB_DataModels.Mappers;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidPrepareDataProvider : PrefillDataProviderBase, IPrefillDataProvider<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType PrefillFormData { get; set; }

        public PrefillData GetPrefillData(string xmlString, string distributionFormId)
        {            
            PrefillFormData = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>(xmlString);

            var prefillData = new PrefillData()
            {
                DataFormatId = PrefillFormData.dataFormatId,
                DataFormatVersion = PrefillFormData.dataFormatVersion,
                Receiver = base.GetReceiver(NabovarselPlanMappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
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
