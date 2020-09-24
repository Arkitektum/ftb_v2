using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Reporter.Strategies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_Reporter
{
    public class ReporterStrategyManager : StrategyManagerBase
    {
        private readonly ITableStorage _tableStorage;
        public ReporterStrategyManager(IConfiguration configuration, ITableStorage tableStorage) : base(configuration)
        {
            _tableStorage = tableStorage;
        }
        public IStrategy<FinishedQueueItem, ReportQueueItem> GetReportStrategy(string serviceCode, IFormLogic formLogic)
        {
            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionReportStrategy(formLogic, _tableStorage);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationReportStrategy(formLogic, _tableStorage);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentReportStrategy(formLogic, _tableStorage);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
