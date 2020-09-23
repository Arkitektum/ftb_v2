using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_PrepareSending;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FtB_FuncPrepareSending
{
    public class SubmittalQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly PrepareSendingStrategyManager _strategyManager;

        public SubmittalQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, PrepareSendingStrategyManager strategyManager)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;
            _strategyManager = strategyManager;
        }
        public List<SendQueueItem> ExecuteProcessingStrategy(SubmittalQueueItem submittalQueueItem)
        {
            try
            {
                string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(submittalQueueItem.ArchiveReference);
                string formatId = _blobOperations.GetFormatIdFromStoredBlob(submittalQueueItem.ArchiveReference);
                IForm formBeingProcessed;
                formBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
                formBeingProcessed.LoadFormData(submittalQueueItem.ArchiveReference);
                formBeingProcessed.ArchiveReference = submittalQueueItem.ArchiveReference;
                
                var strategy = _strategyManager.GetPrepareStrategy(serviceCode, formBeingProcessed);    //channelFactory.CreatePrepareStrategy(formBeingProcessed);
                return strategy.Exceute(); // Alle prefille osv ferdig, og liste av "SendQueueItem" kan returneres

            }
            catch (Azure.RequestFailedException rfEx)
            {
                //{ "The specified container does not exist.\nRequestId:99e1e527-1c78-47a9-93a8-eb2dc7cae764\nTime:2020-09-21T14:20:13.3252740Z\r\nStatus: 404 (The specified container does not exist.)\r\nErrorCode: ContainerNotFound\r\n\r\nHeaders:\r\nServer: Windows-Azure-Blob/1.0,Microsoft-HTTPAPI/2.0\r\nx-ms-request-id: 99e1e527-1c78-47a9-93a8-eb2dc7cae764\r\nx-ms-version: 2019-12-12\r\nx-ms-error-code: ContainerNotFound\r\nDate: Mon, 21 Sep 2020 14:20:13 GMT\r\nContent-Length: 225\r\nContent-Type: application/xml\r\n"}))
                throw rfEx;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
