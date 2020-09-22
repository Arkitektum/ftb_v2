using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Reporter.Strategies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_Preparator
{
    public class ReporterStrategyManager : StrategyManagerBase
    {
        public ReporterStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy GetPrepareStrategy(string serviceCode, IForm form)
        {
            if (_distributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionReportStrategy(form);
            }
            else if (_notificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationReportStrategy(form);
            }
            else if (_shipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentReportStrategy(form);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
