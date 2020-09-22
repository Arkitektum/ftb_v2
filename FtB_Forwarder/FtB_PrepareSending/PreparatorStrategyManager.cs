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
    public class PreparatorStrategyManager : StrategyManagerBase
    {
        public PreparatorStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy<SendQueueItem> GetPrepareStrategy(string serviceCode, IForm form)
        {
            if (_distributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionPrepareStrategy(form);
            }
            else if (_notificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationPrepareStrategy(form);
            }
            else if (_shipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentPrepareStrategy(form);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
