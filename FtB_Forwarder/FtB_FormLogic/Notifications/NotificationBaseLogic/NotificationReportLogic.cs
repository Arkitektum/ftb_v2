using Altinn.Common.Interfaces;
using FtB_Common.BusinessModels;
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
    public class NotificationReportLogic<T> : ReportLogic<T>
    {
        private HtmlToPdfConverterHttpClient _htmlToPdfConverterHttpClient;
        private INotificationAdapter _notificationAdapter;

        public NotificationReportLogic(IFormDataRepo repo,
                                       ITableStorage tableStorage,
                                       ILogger log,
                                       INotificationAdapter notificationAdapter,
                                       IBlobOperations blobOperations,
                                       DbUnitOfWork dbUnitOfWork,
                                       IHtmlUtils htmlUtils,
                                       HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient)
            : base(repo, tableStorage, blobOperations, log, dbUnitOfWork)
        {
            _htmlToPdfConverterHttpClient = htmlToPdfConverterHttpClient;
            _notificationAdapter = notificationAdapter;
        }
        public override async Task<string> ExecuteAsync(ReportQueueItem reportQueueItem)
        {
            var returnItem = await base.ExecuteAsync(reportQueueItem);
            //await UpdateReceiverProcessStageAsync(reportQueueItem.ArchiveReference, reportQueueItem.ReceiverSequenceNumber, reportQueueItem.Receiver.Id, ReceiverProcessStageEnum.ReadyForReporting);
            //await AddToReceiverProcessLogAsync(reportQueueItem.ReceiverLogPartitionKey, reportQueueItem.Receiver.Id, ReceiverStatusLogEnum.ReadyForReporting);

            //if (await ReadyForSubmittalReportingAsync(reportQueueItem))
            //{
            //    await SendReceiptToSubmitterWhenAllReceiversAreProcessedAsync(reportQueueItem);
            //}

            return returnItem;
        }
    }
}
