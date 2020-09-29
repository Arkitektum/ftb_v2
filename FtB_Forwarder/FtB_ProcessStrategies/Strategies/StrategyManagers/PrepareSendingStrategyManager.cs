using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FtB_ProcessStrategies
{
    public class PrepareSendingStrategyManager : StrategyManagerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IEnumerable<IStrategy<List<SendQueueItem>, SubmittalQueueItem>> _strategies;

        public PrepareSendingStrategyManager(IConfiguration configuration, ITableStorage tableStorage, IEnumerable<IStrategy<List<SendQueueItem>, SubmittalQueueItem>> strategies) : base(configuration)
        {
            _tableStorage = tableStorage;
            _strategies = strategies;
        }
        public IStrategy<List<SendQueueItem>, SubmittalQueueItem> GetPrepareStrategy(string serviceCode, IFormLogic formLogic)
        {
            StrategyBase strategyBase = null;

            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                strategyBase = _strategies.Where(t => t is DefaultDistributionPrepareStrategy).FirstOrDefault() as DefaultDistributionPrepareStrategy;
                //return new DefaultDistributionPrepareStrategy(formLogic, _tableStorage);
                //return new TestPrepareStrategy( _tableStorage, log);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                //return new DefaultNotificationPrepareStrategy( _tableStorage, log);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                //return new DefaultShipmentPrepareStrategy( _tableStorage, log);
            }
            else
            {
                throw new Exception("Invalid service code");
            }

            strategyBase.FormLogicBeingProcessed = formLogic;
            return strategyBase as IStrategy<List<SendQueueItem>, SubmittalQueueItem>;


        }
         
    }
}
