using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FtB_ProcessStrategies
{
    public class SenderStrategyManager : StrategyManagerBase
    {
        private readonly IEnumerable<IStrategy<ReportQueueItem, SendQueueItem>> _strategies;

        public SenderStrategyManager(IConfiguration configuration, IEnumerable<IStrategy<ReportQueueItem, SendQueueItem>> strategies) : base(configuration)
        {            
            _strategies = strategies;
        }
        public IStrategy<ReportQueueItem, SendQueueItem> GetSendStrategy(string serviceCode, IFormLogic formLogic)
        {
            StrategyBase strategyBase = null;

            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is DefaultDistributionSendStrategy).FirstOrDefault() as DefaultDistributionSendStrategy;
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is DefaultNotificationSendStrategy).FirstOrDefault() as DefaultNotificationSendStrategy;
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is DefaultShipmentSendStrategy).FirstOrDefault() as DefaultShipmentSendStrategy;
            }
            else
            {
                throw new Exception("Invalid service code");
            }

            strategyBase.FormLogicBeingProcessed = formLogic;
            return strategyBase as IStrategy<ReportQueueItem, SendQueueItem>;
        }   
    }
}
