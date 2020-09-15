using FtB_Common;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Mappers;
using FtB_NotificationForwarding;
using FtB_NotificationForwarding.Mappers;
using FtB_ShipmentForwarding;
using FtB_ShipmentForwarding.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_InitiateForwarding
{
    public class ArchivedItemQueueProcessor
    {
        public ArchivedItemQueueProcessor(string archiveReference)
        {
            try
            {
                BlobOperations blob = new BlobOperations(archiveReference);
                string serviceCode = blob.GetServiceCodeFromStoredBlob();
                var channelFactory = FormatIdToChannelMapper.GetChannelFactory(serviceCode);
                string formatId = blob.GetFormatIdFromStoredBlob();
                IForm formBeingProcessed;
                if (channelFactory is DistributionChannelFactory)
                {
                    //var mapper = new FormatIdToDistributionFormMapper2();
                    formBeingProcessed = FormatIdToDistributionFormMapper.GetForm(formatId);
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
