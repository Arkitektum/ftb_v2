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
            var channel = FormatIdToChannelMapper.GetChannel(formatID);
            IForm formBeingProcessed;
            if (channel is DistributionChannelFactory)
            {
                formBeingProcessed = FormatIdToDistributionFormMapper.GetForm(formatID);
            }
            else if (channel is NotificationChannelFactory)
            {
                formBeingProcessed = FormatIdToNotificationFormMapper.GetForm(formatID);
            }
            else //if (channel is ShipmentChannelFactory)
            {
                formBeingProcessed = FormatIdToShipmentFormMapper.GetForm(formatID);
            }
            PrepareStrategyRunner prepareStrategyRunner = new PrepareStrategyRunner(channel, formBeingProcessed);
            prepareStrategyRunner.Execute();
        }
    }
}
