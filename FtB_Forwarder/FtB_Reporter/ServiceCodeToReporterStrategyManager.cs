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
    public class ServiceCodeToReporterStrategyManager : ServiceCodeToStrategyManagerBase
    {

        public ServiceCodeToReporterStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy GetPrepareStrategy(string serviceCode, IForm form)
        {
            List<string> distributionServiceCodeList = Configuration["DistributionServiceCodes"].Split(',').ToList();
            List<string> notificationServiceCodeList = Configuration["NotificationServiceCodes"].Split(',').ToList();
            List<string> shipmentServiceCodeList = Configuration["ShipmentServiceCodes"].Split(',').ToList();

            if (distributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionReportStrategy(form);
            }
            else if (notificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationReportStrategy(form);
            }
            else if (shipmentServiceCodeList.Contains(serviceCode))
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
