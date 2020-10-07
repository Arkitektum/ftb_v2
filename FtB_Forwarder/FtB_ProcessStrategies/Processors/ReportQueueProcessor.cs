using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_FormLogic;
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
        
        private readonly IEnumerable<IMessageManager> _messageManagers;
        private readonly ILogger _log;

        public ReportQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations
                                    , IEnumerable<IMessageManager> messageManagers, ILogger<ReportQueueProcessor> log)
        {
            _blobOperations = blobOperations;
            
            _messageManagers = messageManagers;
            _log = log;
            _formatIdToFormMapper = formatIdToFormMapper;
        }

        public FinishedQueueItem ExecuteProcessingStrategy(ReportQueueItem reportQueueItem)
        {
            string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(reportQueueItem.ArchiveReference);
            string formatId = _blobOperations.GetFormatIdFromStoredBlob(reportQueueItem.ArchiveReference);

            var formLogicBeingProcessed = _formatIdToFormMapper.GetForm<FinishedQueueItem, ReportQueueItem>(formatId, FormLogicProcessingContext.Report);
            _log.LogDebug($"{GetType().Name}: LoadFormData for ArchiveReference {reportQueueItem.ArchiveReference} and {reportQueueItem.Receiver.Id}....");
                        
            return formLogicBeingProcessed.Execute(reportQueueItem);
        }
    }
}
