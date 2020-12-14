using Altinn.Common.Interfaces;
using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6326", DataFormatVersion = "44843", ProcessingContext = FormLogicProcessingContext.Report)]
    public class SvarVarselOppstartPlanarbeidReportLogic : NotificationReportLogic<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        private readonly IHtmlUtils _htmlUtils;

        public SvarVarselOppstartPlanarbeidReportLogic(IFormDataRepo repo,
                                                       ITableStorage tableStorage,
                                                       ILogger<VarselOppstartPlanarbeidReportLogic> log,
                                                       IBlobOperations blobOperations,
                                                       INotificationAdapter notificationAdapter,
                                                       DbUnitOfWork dbUnitOfWork,
                                                       IHtmlUtils htmlUtils,
                                                       HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient)
            : base(repo, tableStorage, log, notificationAdapter, blobOperations, dbUnitOfWork, htmlUtils, htmlToPdfConverterHttpClient)
        {
            _htmlUtils = htmlUtils;
        }

        public override async Task<string> ExecuteAsync(ReportQueueItem reportQueueItem)
        {
            var returnValue = await base.ExecuteAsync(reportQueueItem);
            //Log comment

            return returnValue;
        }
    }
}
