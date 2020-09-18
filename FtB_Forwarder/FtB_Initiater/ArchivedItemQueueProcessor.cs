using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_DistributionForwarding;
using FtB_NotificationForwarding;
using FtB_ShipmentForwarding;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FtB_InitiateForwarding
{
    public class ArchivedItemQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToDistribution;
        private readonly IBlobOperations _blobOperations;

        public ArchivedItemQueueProcessor(FormatIdToFormMapper formatIdToDistribution, IBlobOperations blobOperations)
        {
            _formatIdToDistribution = formatIdToDistribution;
            _blobOperations = blobOperations;
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

                var channelFactory = FormatIdToChannelMapper.GetChannelFactory(serviceCode);
                IForm formBeingProcessed;
                formBeingProcessed = _formatIdToDistribution.GetForm(formatId);
                formBeingProcessed.LoadFormData(archiveReference);
                IStrategy strategy = channelFactory.CreatePrepareStrategy(formBeingProcessed);
                strategy.Exceute();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
