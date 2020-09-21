using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_Preparator;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FtB_FuncPrepareProcess
{
    public class ArchivedItemQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToFormMapper;
        private readonly IBlobOperations _blobOperations;
        private readonly ServiceCodeToPrepareStrategyManager _strategyManager;

        public ArchivedItemQueueProcessor(FormatIdToFormMapper formatIdToFormMapper, IBlobOperations blobOperations, ServiceCodeToPrepareStrategyManager strategyManager)
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
            catch (Exception)
            {

                throw;
            }

        }
    }
}
