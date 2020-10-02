using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
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

        public ReportQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, ReporterStrategyManager strategyManager
                                    , IEnumerable<IMessageManager> messageManagers, ILogger<ReportQueueProcessor> log)
        {
            _blobOperations = blobOperations;
            _strategyManager = strategyManager;
            _messageManagers = messageManagers;
            _log = log;
            _formatIdToFormMapper = formatIdToFormMapper;
        }

        public FinishedQueueItem ExecuteProcessingStrategy(ReportQueueItem reportQueueItem)
        {
            string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(reportQueueItem.ArchiveReference);
            string formatId = _blobOperations.GetFormatIdFromStoredBlob(reportQueueItem.ArchiveReference);
            IFormLogic formLogicBeingProcessed;
            formLogicBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
            _log.LogInformation($"{GetType().Name}: LoadFormData for ArchiveReference {reportQueueItem.ArchiveReference}....");
            formLogicBeingProcessed.LoadFormData(reportQueueItem.ArchiveReference);

            var strategy = _strategyManager.GetReportStrategy(serviceCode, formLogicBeingProcessed);
            return strategy.Exceute(reportQueueItem);
        }
    }
}
