using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Storage;
using FtB_FormLogic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_ProcessStrategies
{
    public class SubmittalQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;

        private readonly ILogger _log;

        public SubmittalQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations
                                    , ILogger<SubmittalQueueProcessor> log)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;

            _log = log;
        }
        public async Task<IEnumerable<SendQueueItem>> ExecuteProcessingStrategy(SubmittalQueueItem submittalQueueItem)
        {
            try
            {
                //Check if queue item has already been received



                //string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(submittalQueueItem.ArchiveReference);
                string formatId = await _blobOperations.GetFormatIdFromStoredBlob(submittalQueueItem.ArchiveReference);
                var formLogicBeingProcessed = _formatIdToFormMapper.GetFormLogic<IEnumerable<SendQueueItem>, SubmittalQueueItem>(formatId, FormLogicProcessingContext.Prepare);
                
                return await formLogicBeingProcessed.Execute(submittalQueueItem);
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
