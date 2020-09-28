using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class ReportQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly ReporterStrategyManager _strategyManager;
        private readonly IEnumerable<IMessageManager> _messageManagers;
        private readonly ILogger _log;

        public ReportQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations
                                    , ReporterStrategyManager strategyManager, IEnumerable<IMessageManager> messageManagers, ILogger log)
        {
            _blobOperations = blobOperations;
            _strategyManager = strategyManager;
            _messageManagers = messageManagers;
            _log = log;
            _formatIdToFormMapper = formatIdToFormMapper;
        }

        public FinishedQueueItem ExecuteProcessingStrategy(ReportQueueItem reportQueueItem, ILogger log)
        {
            string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(reportQueueItem.ArchiveReference);
            string formatId = _blobOperations.GetFormatIdFromStoredBlob(reportQueueItem.ArchiveReference);
            IFormLogic formBeingProcessed;
            formBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
            formBeingProcessed.LoadFormData(reportQueueItem.ArchiveReference);

            var strategy = _strategyManager.GetReportStrategy(serviceCode, formBeingProcessed, _messageManagers, log);
            return strategy.Exceute(reportQueueItem);
        }
    }
}
