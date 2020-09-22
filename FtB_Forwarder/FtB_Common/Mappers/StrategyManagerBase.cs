using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace FtB_Common.Mappers
{
    public abstract class StrategyManagerBase
    {
        protected List<string> _distributionServiceCodeList = new List<string>();
        protected List<string> _notificationServiceCodeList = new List<string>();
        protected List<string> _shipmentServiceCodeList = new List<string>();

        public StrategyManagerBase(IConfiguration configuration)
        {
            _distributionServiceCodeList = configuration["DistributionServiceCodes"].Split(',').ToList();
            _notificationServiceCodeList = configuration["NotificationServiceCodes"].Split(',').ToList();
            _shipmentServiceCodeList = configuration["ShipmentServiceCodes"].Split(',').ToList();
        }
    }
}
