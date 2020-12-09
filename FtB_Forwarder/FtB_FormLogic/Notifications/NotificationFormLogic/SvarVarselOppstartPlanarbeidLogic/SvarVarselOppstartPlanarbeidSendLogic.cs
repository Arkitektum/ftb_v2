using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6326", DataFormatVersion = "44843", ProcessingContext = FormLogicProcessingContext.Send)]
    public class SvarVarselOppstartPlanarbeidSendLogic : NotificationSendLogic<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public SvarVarselOppstartPlanarbeidSendLogic(IFormDataRepo repo,
                                                 ITableStorage tableStorage,
                                                 IBlobOperations blobOperations,
                                                 ILogger<VarselOppstartPlanarbeidSendLogic> log,
                                                 DbUnitOfWork dbUnitOfWork
            )
            : base(repo, tableStorage, log, dbUnitOfWork)
        {

        }
    }
}
