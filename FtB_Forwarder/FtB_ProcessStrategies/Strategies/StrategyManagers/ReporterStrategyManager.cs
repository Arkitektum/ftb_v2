using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace FtB_ProcessStrategies
{
    public class ReporterStrategyManager : StrategyManagerBase
    {
        private readonly Func<StrategyTypeEnum, IStrategy<FinishedQueueItem, ReportQueueItem>> _serviceProvider;

        public ReporterStrategyManager(IConfiguration configuration, Func<StrategyTypeEnum, IStrategy<FinishedQueueItem, ReportQueueItem>> serviceProvider) : base(configuration)
        {            
            _serviceProvider = serviceProvider;
        }
        public IStrategy<FinishedQueueItem, ReportQueueItem> GetReportStrategy(string serviceCode, IFormLogic formLogic)
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
            return strategyBase as IStrategy<FinishedQueueItem, ReportQueueItem>;
        }      
    }
}
