using FtB_Common.Adapters;
using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Report)]
    public class VarselOppstartPlanarbeidReportLogic : DistributionReportLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public ILogger<VarselOppstartPlanarbeidReportLogic> _log { get; }

        public VarselOppstartPlanarbeidReportLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger<VarselOppstartPlanarbeidReportLogic> log
                , IEnumerable<IMessageManager> messageManagers) : base(repo, tableStorage, log, messageManagers)
        {
            _log = log;
        }
        public override void SetSubmitterReportContent(SubmittalEntity submittalEntity)
        {
            SubmitterReport.Subject = "Kvittering for innsending";
            var body = new StringBuilder();
            body.Append($"Innsending for {submittalEntity.PartitionKey.ToUpper()}{Environment.NewLine}");
            body.Append($"Antall mottakere: {submittalEntity.ReceiverCount}{Environment.NewLine}");
            body.Append($"Antall prosesserte: {submittalEntity.ProcessedCount}{Environment.NewLine}");
            body.Append($"Antall vellykkede utsendinger: {submittalEntity.SuccessCount}{Environment.NewLine}");
            body.Append($"Antall med reservasjon mot digital kommunikasjon: {submittalEntity.DigitalDisallowmentCount}{Environment.NewLine}");
            body.Append($"Antall som feilet ved utsending: {submittalEntity.FailedCount}");
            SubmitterReport.Body = body.ToString();
            //_log.LogDebug($"{GetType().Name}: Body: {SubmitterReport.Body}");
        }

        public override string Execute(ReportQueueItem reportQueueItem)
        {
            return base.Execute(reportQueueItem);
        }

    }
}
