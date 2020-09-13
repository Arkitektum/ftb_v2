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
            string formatID = BlobOperations.GetFormatIdFromStoredBlob(archiveReference);
            var channelFactory = FormatIdToChannelMapper.GetChannelFactory(formatID);
            IForm formBeingProcessed;
            if (channelFactory is DistributionChannelFactory)
            {
                formBeingProcessed = FormatIdToDistributionFormMapper.GetForm(formatID);
            }
            else if (channelFactory is NotificationChannelFactory)
            {
                formBeingProcessed = FormatIdToNotificationFormMapper.GetForm(formatID);
            }
            else if (channelFactory is ShipmentChannelFactory)
            {
                formBeingProcessed = FormatIdToShipmentFormMapper.GetForm(formatID);
            }
            else
            {
                throw new ArgumentException("Invalid channelFactory.");
            }

            IStrategy strategy = channelFactory.CreatePrepareStrategy(formBeingProcessed);
            strategy.Exceute();
        }
    }
}
