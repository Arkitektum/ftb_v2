using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_PrepareSending;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FtB_FuncPrepareSending
{
    public class ArchivedItemQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly PreparatorStrategyManager _strategyManager;

        public ArchivedItemQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, PreparatorStrategyManager strategyManager)
        {
            _formatIdToFormMapper = formatIdToFormMapper;
            _blobOperations = blobOperations;
            _strategyManager = strategyManager;
        }
        public void ExecuteProcessingStrategy(string archiveReference)
        {
            try
            {
                string serviceCode = _blobOperations.GetServiceCodeFromStoredBlob(archiveReference);
                string formatId = _blobOperations.GetFormatIdFromStoredBlob(archiveReference);

                //Berre for å teste DI.. :-)
                //string serviceCode = "4655";
                //string formatId = "1234";
                IForm formBeingProcessed;
                formBeingProcessed = _formatIdToFormMapper.GetForm(formatId);
                formBeingProcessed.LoadFormData(archiveReference);

                IStrategy strategy = _strategyManager.GetPrepareStrategy(serviceCode, formBeingProcessed);    //channelFactory.CreatePrepareStrategy(formBeingProcessed);
                strategy.Exceute();
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
