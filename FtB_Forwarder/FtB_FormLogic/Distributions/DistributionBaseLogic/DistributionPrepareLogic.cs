using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    public class DistributionPrepareLogic<T> : PrepareLogic<T>
    {
        private readonly ILogger log;

        public DistributionPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
            this.log = log;
        }

        public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            log.LogInformation("Jau");
            return base.Exceute(submittalQueueItem);
        }
    }





    //[FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    //public class VarselOppstartPlanarbeidSendLogic : VarselOppstartPlanarbeidLogic, ISendResultProvider
    //{
    //    private readonly ISendStrategy sendStrategy;
    //    private readonly IFormMapper<NabovarselPlanType, SvarPaaNabovarselPlanType> formMapper;

    //    //Problemstilling:
    //    // Skal mappe frå ein type til ein anna og sende den
    //    // Mapping er unik for denne, men utsending er felles for fleire

    //    public VarselOppstartPlanarbeidSendLogic(ISendStrategy sendStrategy, IFormMapper<NabovarselPlanType, SvarPaaNabovarselPlanType> formMapper)
    //    {   
    //        this.sendStrategy = sendStrategy;
    //        this.formMapper = formMapper;
    //    }

    //    private ReportQueueItem reportItem;
    //    public override void ExecuteStrategy(object strategyTrigger)
    //    {
    //        formMapper.Map(base.)
    //        reportItem = sendStrategy.Execute(strategyTrigger as SendQueueItem);
    //    }

    //    public ReportQueueItem GetResult()
    //    {
    //        return reportItem;
    //    }
    //}

    //public class VarselOppstartPlanarbeidReportLogic : VarselOppstartPlanarbeidLogic
    //{
    //    private readonly IReportStrategy reportStrategy;

    //    public VarselOppstartPlanarbeidReportLogic(IReportStrategy reportStrategy)
    //    {
    //        this.reportStrategy = reportStrategy;
    //    }

    //    public override void ExecuteStrategy(object strategyTrigger)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
