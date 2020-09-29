using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_ProcessStrategies
{
    public class SenderStrategyManager : StrategyManagerBase
    {
        private readonly ITableStorage _tableStorage;
        public SenderStrategyManager(IConfiguration configuration, ITableStorage tableStorage) : base(configuration)
        {
            _tableStorage = tableStorage;
        }
        public IStrategy<ReportQueueItem, SendQueueItem> GetSendStrategy(string serviceCode, IFormLogic formLogic, ILogger log)
        {
            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionSendStrategy(formLogic, _tableStorage, log);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationSendStrategy(formLogic, _tableStorage, log);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentSendStrategy(formLogic, _tableStorage, log);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
