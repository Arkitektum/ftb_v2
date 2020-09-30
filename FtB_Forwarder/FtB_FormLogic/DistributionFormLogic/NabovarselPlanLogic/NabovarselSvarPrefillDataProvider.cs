﻿using FtB_Common.Interfaces;
using FtB_Common.Utils;
using FtB_DataModels.Mappers;
using no.kxml.skjema.dibk.nabovarselsvarPlan;

namespace FtB_FormLogic
{
    public class NabovarselSvarPrefillDataProvider : PrefillDataProviderBase, IPrefillDataProvider<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public SvarPaaNabovarselPlanType PrefillFormData { get; set; }

        public PrefillData GetPrefillData(string xmlString, string identifier)
        {
            PrefillFormData = SerializeUtil.DeserializeFromString<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>(xmlString);

            var prefillData = new PrefillData()
            {
                DataFormatId = PrefillFormData.dataFormatId,
                DataFormatVersion = PrefillFormData.dataFormatVersion,
                Reciever = base.GetReceiver(NabovarselPlanMappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
                DistributionFormId = identifier,
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
