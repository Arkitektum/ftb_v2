using Altinn.Common.Models;
using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using FtB_DataModels.Mappers;
using no.kxml.skjema.dibk.nabovarselPlan;
using System;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    //public class VarselOppstartPlanarbeidAltinn3SendDataProvider : SendDataProviderBase, IDistributionDataMapper<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    //{
    //    //{
    //    //    public FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType PrefillFormData { get; set; }
    //    //    public string PrefillFormName { get { return "Uttalelse til oppstart av reguleringsplanarbeid"; } }
    //    //    public string ExternalSystemMainReference
    //    //    {
    //    //        get
    //    //        {
    //    //            if (PrefillFormData == null)
    //    //                throw new System.NullReferenceException("PrefillFormData is null.");
    //    //            else
    //    //                return PrefillFormData.hovedinnsendingsnummer;
    //    //        }
    //    //        set
    //    //        {
    //    //            if (PrefillFormData == null)
    //    //                throw new System.NullReferenceException("PrefillFormData is null.");
    //    //            else
    //    //                PrefillFormData.hovedinnsendingsnummer = value;

    //    //        }
    //    //    }
    //    //    public string ExternalSystemSubReference
    //    //    {
    //    //        get
    //    //        {
    //    //            if (PrefillFormData == null)
    //    //                throw new System.NullReferenceException("PrefillFormData is null.");
    //    //            else
    //    //                return PrefillFormData.beroertPart.systemReferanse;
    //    //        }
    //    //    }
    //    //    public string PrefillServiceCode { get => "5419"; }
    //    //    public string PrefillServiceEditionCode { get => "1"; }

    //    //public AltinnDistributionMessage GetDistributionMessage(IEnumerable<IPrefillData> prefillData, NabovarselPlanType mainFormData, Guid distributionFormId, string archiveReference)
    //    //{
            

    //    //    var prefillData = new AltinnDistributionMessage()
    //    //    {
    //    //        PrefillDataFormatId = PrefillFormData.dataFormatId,
    //    //        PrefillDataFormatVersion = PrefillFormData.dataFormatVersion,                
    //    //        DistributionFormReferenceId = distributionFormId,
    //    //        PrefillServiceCode = PrefillServiceCode,
    //    //        PrefillServiceEditionCode = PrefillServiceEditionCode,
    //    //        PrefilledXmlDataString = prefillXmlString,
    //    //        DaysValid = 14,
    //    //        DueDate = null
    //    //    };

    //    //    prefillData.NotificationMessage = new AltinnNotificationMessage()
    //    //    {
    //    //        Receiver = base.GetReceiver(NabovarselPlanAltinn3Mappers.GetNabovarselReceiverMapper().Map<BerortPart>(PrefillFormData.beroertPart)),
    //    //        ArchiveReference = archiveReference
    //    //    };

    //    //    return prefillData;
    //    //}
    //}
}
