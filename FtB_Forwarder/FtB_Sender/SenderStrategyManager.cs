﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Mappers;
using FtB_Sender.Strategies;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_Sender
{
    public class SenderStrategyManager : StrategyManagerBase
    {
        public SenderStrategyManager(IConfiguration configuration) : base(configuration)
        {
        }
        public IStrategy<ReportQueueItem> GetSendStrategy(string serviceCode, IFormLogic form)
        {
            if (_distributionServiceCodeList.Contains(serviceCode))
            {
                return new DefaultDistributionSendStrategy(form);
            }
            else if (_notificationServiceCodeList.Contains(serviceCode))
            {
                return new DefaultNotificationSendStrategy(form);
            }
            else if (_shipmentServiceCodeList.Contains(serviceCode))
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
