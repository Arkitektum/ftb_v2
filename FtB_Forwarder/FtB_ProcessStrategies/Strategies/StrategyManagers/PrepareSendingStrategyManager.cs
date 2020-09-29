using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class PrepareSendingStrategyManager : StrategyManagerBase
    {
        private readonly Func<StrategyTypeEnum, IStrategy<List<SendQueueItem>, SubmittalQueueItem>> _serviceProvider;

        public PrepareSendingStrategyManager(IConfiguration configuration, Func<StrategyTypeEnum, IStrategy<List<SendQueueItem>, SubmittalQueueItem>> serviceProvider) : base(configuration)
        {
            _serviceProvider = serviceProvider;
        }
        public IStrategy<List<SendQueueItem>, SubmittalQueueItem> GetPrepareStrategy(string serviceCode, IFormLogic formLogic)
        {
            StrategyBase strategyBase = null;

            if (DistributionServiceCodeList.Contains(serviceCode))
                strategyBase = _serviceProvider(StrategyTypeEnum.Distribution) as StrategyBase;
            else if (NotificationServiceCodeList.Contains(serviceCode))
                strategyBase = _serviceProvider(StrategyTypeEnum.Notification) as StrategyBase;
            else if (ShipmentServiceCodeList.Contains(serviceCode))
                strategyBase = _serviceProvider(StrategyTypeEnum.Shipping) as StrategyBase;
            else
                throw new Exception("Invalid service code");

            strategyBase.FormLogicBeingProcessed = formLogic;
            return strategyBase as IStrategy<List<SendQueueItem>, SubmittalQueueItem>;
        }         
    }
}
