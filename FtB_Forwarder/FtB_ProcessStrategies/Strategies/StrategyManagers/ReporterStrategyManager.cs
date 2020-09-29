using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FtB_ProcessStrategies
{
    public class ReporterStrategyManager : StrategyManagerBase
    {
        private readonly IEnumerable<IStrategy<FinishedQueueItem, ReportQueueItem>> _strategies;

        public ReporterStrategyManager(IConfiguration configuration, IEnumerable<IStrategy<FinishedQueueItem, ReportQueueItem>> strategies) : base(configuration)
        {            
            _strategies = strategies;
        }
        public IStrategy<FinishedQueueItem, ReportQueueItem> GetReportStrategy(string serviceCode, IFormLogic formLogic)//, IEnumerable<IMessageManager> messageManagers)
        {
            StrategyBase strategyBase = null;

            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is TestReportStrategy).FirstOrDefault() as TestReportStrategy;
                //return new DefaultDistributionReportStrategy(formLogic, _tableStorage, messageManagers, log);
                //return new TestReportStrategy(_tableStorage, log, messageManagers);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is DefaultNotificationReportStrategy).FirstOrDefault() as DefaultNotificationReportStrategy;
                //return new DefaultNotificationReportStrategy(_tableStorage, log, messageManagers);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is DefaultShipmentReportStrategy).FirstOrDefault() as DefaultShipmentReportStrategy;
                //return new DefaultShipmentReportStrategy( _tableStorage, log, messageManagers);
            }
            else
            {
                throw new Exception("Invalid service code");
            }

            strategyBase.FormLogicBeingProcessed = formLogic;
            return strategyBase as IStrategy<FinishedQueueItem, ReportQueueItem>;
        }
         
    }
}
