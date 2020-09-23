using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Sender.Strategies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_Sender
{
    public class SenderStrategyManager : StrategyManagerBase
    {
        public SenderStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy<ReportQueueItem> GetSendStrategy(string serviceCode, IFormLogic formLogic)
        {
            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionSendStrategy(formLogic);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationSendStrategy(formLogic);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentSendStrategy(formLogic);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
