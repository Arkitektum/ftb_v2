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
        private readonly ITableStorage _tableStorage;

        public PrepareSendingStrategyManager(IConfiguration configuration, ITableStorage tableStorage) : base(configuration)
        {
            _tableStorage = tableStorage;
        }
        public IStrategy<List<SendQueueItem>, SubmittalQueueItem> GetPrepareStrategy(string serviceCode, IFormLogic formLogic)
        {
            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionPrepareStrategy(formLogic, _tableStorage);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationPrepareStrategy(formLogic, _tableStorage);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentPrepareStrategy(formLogic, _tableStorage);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
