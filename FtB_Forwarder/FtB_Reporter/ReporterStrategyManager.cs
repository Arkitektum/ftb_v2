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
        public ReporterStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy<FinishedQueueItem> GetReportStrategy(string serviceCode, IFormLogic formLogic)
        {
            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionReportStrategy(formLogic);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationReportStrategy(formLogic);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentReportStrategy(formLogic);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
