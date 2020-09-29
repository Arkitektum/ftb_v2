﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using Microsoft.Extensions.Logging;

namespace FtB_ProcessStrategies
{
    public class SendQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly SenderStrategyManager _strategyManager;

        public SendQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, SenderStrategyManager strategyManager)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;
            _strategyManager = strategyManager;
        }

        public ReportQueueItem ExecuteProcessingStrategy(SendQueueItem sendQueueItem, ILogger log)
        {
            string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(sendQueueItem.ArchiveReference);
            string formatId = _blobOperations.GetFormatIdFromStoredBlob(sendQueueItem.ArchiveReference);
            IFormLogic formBeingProcessed;
            formBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
            formBeingProcessed.LoadFormData(sendQueueItem.ArchiveReference);
            
            var strategy = _strategyManager.GetSendStrategy(serviceCode, formBeingProcessed, log);
            var result =  strategy.Exceute(sendQueueItem);
            return result;
        }
    }
}
