using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_FormLogic.OTSFormLogic;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_ProcessStrategies
{
    public class SendQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        
        private readonly ILogger _log;

        public SendQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations
                                  , ILogger<SendQueueProcessor> log)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;
            
            _log = log;
        }

        public ReportQueueItem ExecuteProcessingStrategy(SendQueueItem sendQueueItem)
        {
            try
            {
                string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(sendQueueItem.ArchiveReference);
                string formatId = _blobOperations.GetFormatIdFromStoredBlob(sendQueueItem.ArchiveReference);
                
                var formLogicBeingProcessed = _formatIdToFormMapper.GetForm<ReportQueueItem, SendQueueItem>(formatId, FormLogicProcessingContext.Send);
                _log.LogDebug($"{GetType().Name}: LoadFormData for ArchiveReference {sendQueueItem.ArchiveReference} and {sendQueueItem.Receiver.Id}....");

                var result = formLogicBeingProcessed.Execute(sendQueueItem);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
