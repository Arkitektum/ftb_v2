using FtB_Common.BusinessModels;
using FtB_Common.Exceptions;
using FtB_Common.FormLogic;
//using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_FormLogic;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FtB_ProcessStrategies
{
    public class SendQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly ILogger _log;
        private readonly DbUnitOfWork _dbUnitOfWork;

        public SendQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, ILogger<SendQueueProcessor> log, DbUnitOfWork dbUnitOfWork)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;
            _log = log;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public async Task<ReportQueueItem> ExecuteProcessingStrategyAsync(SendQueueItem sendQueueItem)
        {
            try
            {
                _log.LogDebug($"{GetType().Name}: _dbUnitOfWork hash {0}", _dbUnitOfWork.GetHashCode());
                
                //_dbUnitOfWork.SetArchiveReference(sendQueueItem.ArchiveReference); //Moved to DistributionSendLogic
                string serviceCode = await _blobOperations.GetServiceCodeFromStoredBlob(sendQueueItem.ArchiveReference);
                string formatId = await _blobOperations.GetFormatIdFromStoredBlob(sendQueueItem.ArchiveReference);
                var formLogicBeingProcessed = _formatIdToFormMapper.GetFormLogic<ReportQueueItem, SendQueueItem>(formatId, FormLogicProcessingContext.Send);
                var result = await formLogicBeingProcessed.ExecuteAsync(sendQueueItem);
                return result;
            }
            catch (DistributionSendExeception dsex)
            {
                _log.LogError($"{GetType().Name}: An error occured while distribution for archiveReference {sendQueueItem?.ArchiveReference}. DistributionFormReferenceId {dsex.DistributionFormReferenceId}");
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"{GetType().Name}: An error occured while processing: {0}", sendQueueItem?.ArchiveReference);
                throw;
            }
            finally
            {
                //TODO: ???Kanskje skummelt å lagre alle endringer i distributionforms, metadata, dersom ting går feil? Alle Logentries er ok å lagre..
                //?????????????????
                await _dbUnitOfWork.SaveLogEntries();
            }
        }
    }
}
