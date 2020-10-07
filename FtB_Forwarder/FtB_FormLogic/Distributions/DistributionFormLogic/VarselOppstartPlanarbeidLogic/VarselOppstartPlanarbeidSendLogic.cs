using FtB_Common.Adapters;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    public class VarselOppstartPlanarbeidSendLogic : DistributionSendLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        //private VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper prefillMapper;
        private VarselOppstartPlanarbeidPreparePrefillMapper prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger<VarselOppstartPlanarbeidSendLogic> log, IPrefillAdapter prefillAdapter) : base(repo, tableStorage, log, prefillAdapter)
        {
            //prefillMapper = new VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper();
            prefillMapper = new VarselOppstartPlanarbeidPreparePrefillMapper();
        }

        protected override void MapPrefillData(string receiverId)
        {
            prefillMapper.Map(base.FormData, receiverId);
            //base.PrefillData = new VarselOppstartPlanarbeidPrepareAltinn3PrefillDataProvider().GetPrefillData(prefillMapper.FormDataString, Guid.NewGuid().ToString());
            base.PrefillData = new VarselOppstartPlanarbeidPrepareDataProvider().GetPrefillData(prefillMapper.FormDataString, Guid.NewGuid().ToString());
        }
    }
}
