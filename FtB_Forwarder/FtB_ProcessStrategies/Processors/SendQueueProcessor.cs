﻿using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
//using FtB_Common.Mappers;
using FtB_Common.Storage;
using Ftb_DbRepository;
using FtB_FormLogic;
using Microsoft.Extensions.Logging;
using System;

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

        public ReportQueueItem ExecuteProcessingStrategy(SendQueueItem sendQueueItem)
        {
            try
            {
                //string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(sendQueueItem.ArchiveReference);
                string formatId = _blobOperations.GetFormatIdFromStoredBlob(sendQueueItem.ArchiveReference);
                var formLogicBeingProcessed = _formatIdToFormMapper.GetFormLogic<ReportQueueItem, SendQueueItem>(formatId, FormLogicProcessingContext.Send);

                var result = formLogicBeingProcessed.Execute(sendQueueItem);                
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                //?????????????????
                _dbUnitOfWork.Save();
            }


        }
    }
}
