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
        private NabovarselPlanPrefillMapper prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IPrefillAdapter prefillAdapter) : base(repo, tableStorage, log, prefillAdapter)
        {
            prefillMapper = new NabovarselPlanPrefillMapper();
        }

        protected override void MapPrefillData(string receiverId)
        {
            prefillMapper.Map(base.FormData, "");
            base.PrefillData = new NabovarselSvarPrefillDataProvider().GetPrefillData(prefillMapper.FormDataString, Guid.NewGuid().ToString());
        }
    }
}
