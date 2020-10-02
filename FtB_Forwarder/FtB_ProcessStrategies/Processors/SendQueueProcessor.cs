using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_ProcessStrategies
{
    public class SendQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly SenderStrategyManager _strategyManager;
        private readonly ILogger _log;

        public SendQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, SenderStrategyManager strategyManager
                                  , ILogger<SendQueueProcessor> log)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;
            _strategyManager = strategyManager;
            _log = log;
        }

        public ReportQueueItem ExecuteProcessingStrategy(SendQueueItem sendQueueItem)
        {
            string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(sendQueueItem.ArchiveReference);
            string formatId = _blobOperations.GetFormatIdFromStoredBlob(sendQueueItem.ArchiveReference);
            IFormLogic formLogicBeingProcessed;
            formLogicBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
            _log.LogInformation($"{GetType().Name}: LoadFormData for ArchiveReference {sendQueueItem.ArchiveReference}....");
            formLogicBeingProcessed.LoadFormData(sendQueueItem.ArchiveReference);
            
            var strategy = _strategyManager.GetSendStrategy(serviceCode, formLogicBeingProcessed);
            var result =  strategy.Exceute(sendQueueItem);
            return result;
        }
    }
}
