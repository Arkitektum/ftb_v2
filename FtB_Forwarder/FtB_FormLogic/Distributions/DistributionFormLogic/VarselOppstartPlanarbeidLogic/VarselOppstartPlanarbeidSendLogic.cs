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
        private readonly VarselOppstartPlanarbeidPrefillMapper _prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo, 
                                                 ITableStorage tableStorage, 
                                                 ILogger<VarselOppstartPlanarbeidSendLogic> log, 
                                                 IDistributionAdapter distributionAdapter, 
                                                 IDistributionDataMapper<SvarPaaNabovarselPlanType, NabovarselPlanType> distributionDataMapper,
                                                 VarselOppstartPlanarbeidPrefillMapper prefillMapper) 
            : base(repo, tableStorage, log, distributionAdapter)
        {   
            _distributionDataMapper = distributionDataMapper;
            this._prefillMapper = prefillMapper;
        }

        protected override void MapPrefillData(string receiverId)
        {
            _prefillMapper.Map(base.FormData, receiverId);
            base.DistributionMessage = _distributionDataMapper.GetDistributionMessage(_prefillMapper.FormDataString, base.FormData, Guid.NewGuid().ToString(), base.ArchiveReference);
        }
    }
}
