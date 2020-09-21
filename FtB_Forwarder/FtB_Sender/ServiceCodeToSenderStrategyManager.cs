using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Sender.Strategies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_Preparator
{
    public class ServiceCodeToSenderStrategyManager : ServiceCodeToStrategyManagerBase
    {
        //private readonly IConfiguration Configuration;

        public ServiceCodeToSenderStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy GetPrepareStrategy(string serviceCode, IForm form)
        {
            List<string> distributionServiceCodeList = Configuration["DistributionServiceCodes"].Split(',').ToList();
            List<string> notificationServiceCodeList = Configuration["NotificationServiceCodes"].Split(',').ToList();
            List<string> shipmentServiceCodeList = Configuration["ShipmentServiceCodes"].Split(',').ToList();

            if (distributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionSendStrategy(form);
            }
            else if (notificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationSendStrategy(form);
            }
            else if (shipmentServiceCodeList.Contains(serviceCode))
            {
                return new DefaultShipmentSendStrategy(form);
            }
            else
            {
                throw new Exception("Invalid service code");
            }


        }
         
    }
}
