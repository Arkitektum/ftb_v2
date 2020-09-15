using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Common.Storage;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Mappers;
using FtB_NotificationForwarding;
using FtB_NotificationForwarding.Mappers;
using FtB_ShipmentForwarding;
using FtB_ShipmentForwarding.Mappers;
using System;

namespace FtB_InitiateForwarding
{
    public class ArchivedItemQueueProcessor
    {
        private readonly FormatIdToFormMapper _formatIdToDistribution;

        public ArchivedItemQueueProcessor(FormatIdToFormMapper formatIdToDistribution)
        {
            _formatIdToDistribution = formatIdToDistribution;
        }
        public void ExecuteProcessingStrategy(string archiveReference)
        {
            try
            {
                BlobOperations blob = new BlobOperations(archiveReference);
                string serviceCode = blob.GetServiceCodeFromStoredBlob();
                string formatId = blob.GetFormatIdFromStoredBlob();

                //Berre for å teste DI.. :-)
                //string serviceCode = "4655";
                //string formatId = "1234";

                var channelFactory = FormatIdToChannelMapper.GetChannelFactory(serviceCode);
                IForm formBeingProcessed;
                if (channelFactory is DistributionChannelFactory)
                {
                    formBeingProcessed = _formatIdToDistribution.GetForm(formatId);
                }
                else if (channelFactory is NotificationChannelFactory)
                {
                    formBeingProcessed = FormatIdToNotificationFormMapper.GetForm(formatId);
                }
                else if (channelFactory is ShipmentChannelFactory)
                {
                    formBeingProcessed = FormatIdToShipmentFormMapper.GetForm(formatId);
                }
                else
                {
                    throw new ArgumentException("Invalid channelFactory.");
                }

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
