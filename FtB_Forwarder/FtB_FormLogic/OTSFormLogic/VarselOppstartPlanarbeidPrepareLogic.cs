using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic.OTSFormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Prepare)]
    public class VarselOppstartPlanarbeidPrepareLogic : DistributionPrepareLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public VarselOppstartPlanarbeidPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
        }
    }
}
