using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Report)]
    public class VarselOppstartPlanarbeidReportLogic : DistributionReportLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public VarselOppstartPlanarbeidReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger<VarselOppstartPlanarbeidReportLogic> log) : base(repo, tableStorage, log)
        {
        }
    }
}
