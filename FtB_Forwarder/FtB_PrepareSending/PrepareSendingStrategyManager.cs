using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_PrepareSending.Strategies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_PrepareSending
{
    public class PrepareSendingStrategyManager : StrategyManagerBase
    {
        public PrepareSendingStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy<SendQueueItem> GetPrepareStrategy(string serviceCode, IFormLogic formLogic)
        {
            if (DistributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionPrepareStrategy(formLogic);
            }
            else if (NotificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationPrepareStrategy(formLogic);
            }
            else if (ShipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentPrepareStrategy(formLogic);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
