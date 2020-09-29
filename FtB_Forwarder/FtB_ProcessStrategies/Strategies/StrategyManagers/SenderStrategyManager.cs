using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace FtB_ProcessStrategies
{
    public class SenderStrategyManager : StrategyManagerBase
    {
        private readonly Func<StrategyTypeEnum, IStrategy<ReportQueueItem, SendQueueItem>> serviceProvider;

        public SenderStrategyManager(IConfiguration configuration, Func<StrategyTypeEnum, IStrategy<ReportQueueItem, SendQueueItem>> serviceProvider) : base(configuration)
        {            
            this.serviceProvider = serviceProvider;
        }
        public IStrategy<ReportQueueItem, SendQueueItem> GetSendStrategy(string serviceCode, IFormLogic formLogic)
        {
            StrategyBase strategyBase = null;

            if (DistributionServiceCodeList.Contains(serviceCode))
                strategyBase = serviceProvider(StrategyTypeEnum.Distribution) as StrategyBase;
            else if (NotificationServiceCodeList.Contains(serviceCode))
                strategyBase = serviceProvider(StrategyTypeEnum.Notification) as StrategyBase;
            else if (ShipmentServiceCodeList.Contains(serviceCode))
                strategyBase = serviceProvider(StrategyTypeEnum.Shipping) as StrategyBase;
            else
                throw new Exception("Invalid service code");

            strategyBase.FormLogicBeingProcessed = formLogic;
            return strategyBase as IStrategy<ReportQueueItem, SendQueueItem>;
        }   
    }
}
