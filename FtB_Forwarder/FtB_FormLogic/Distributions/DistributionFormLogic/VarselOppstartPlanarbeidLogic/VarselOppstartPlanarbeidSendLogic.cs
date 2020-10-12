using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using no.kxml.skjema.dibk.nabovarselPlan;
using no.kxml.skjema.dibk.nabovarselsvarPlan;
using System;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    public class VarselOppstartPlanarbeidSendLogic : DistributionSendLogic<NabovarselPlanType>
    {
        private readonly IDistributionDataMapper<SvarPaaNabovarselPlanType, NabovarselPlanType> _distributionDataMapper;

        //private VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper prefillMapper;
        private VarselOppstartPlanarbeidPreparePrefillMapper _prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo, 
                                                 ITableStorage tableStorage, 
                                                 ILogger<VarselOppstartPlanarbeidSendLogic> log, 
                                                 IDistributionAdapter distributionAdapter, IDistributionDataMapper<SvarPaaNabovarselPlanType, NabovarselPlanType> distributionDataMapper) 
            : base(repo, tableStorage, log, distributionAdapter)
        {
            //prefillMapper = new VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper();
            _prefillMapper = new VarselOppstartPlanarbeidPreparePrefillMapper();
            _distributionDataMapper = distributionDataMapper;
        }

        protected override void MapPrefillData(string receiverId)
        {
            _prefillMapper.Map(base.FormData, receiverId);
            base.DistributionMessage = _distributionDataMapper.GetDistributionMessage(_prefillMapper.FormDataString, base.FormData, Guid.NewGuid().ToString());
                                    
        }
    }

    //public interface IDistributionMessageProvider<T>
    //{
    //    Altinn.Common.Models.MessageDataType CreateDistributionMessage(T formData);
    //}

    //public class VarselOppstartPlanarbeidMessageProvider : IDistributionMessageProvider<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    //{
    //    public MessageDataType CreateDistributionMessage(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType formData)
    //    {
    //        var retVal = new MessageDataType();
            
    //        //Create body, title and summary
           

    //        return retVal;
    //    }
    //}
}
