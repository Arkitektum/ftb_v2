using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_Sender;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FuncSender
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

        public ReportQueueItem ExecuteProcessingStrategy(SendQueueItem sendQueueItem)
        {
            string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(sendQueueItem.ArchiveReference);
            string formatId = _blobOperations.GetFormatIdFromStoredBlob(sendQueueItem.ArchiveReference);
            IFormLogic formBeingProcessed;
            formBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
            formBeingProcessed.LoadFormData(sendQueueItem.ArchiveReference);
            
            var strategy = _strategyManager.GetSendStrategy(serviceCode, formBeingProcessed);
            var result =  strategy.Exceute(sendQueueItem);
            return result;
        }
    }
}
