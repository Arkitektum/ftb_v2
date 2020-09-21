using FtB_Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace FtB_Common.Mappers
{
    public abstract class ServiceCodeToStrategyManagerBase
    {
        protected List<string> _distributionServiceCodeList;
        protected List<string> _notificationServiceCodeList;
        protected List<string> _shipmentServiceCodeList;

        protected readonly IConfiguration Configuration;

        public ServiceCodeToStrategyManagerBase(IConfiguration configuration)
        {
            _distributionServiceCodeList = Configuration["DistributionServiceCodes"].Split(',').ToList();
            _notificationServiceCodeList = Configuration["NotificationServiceCodes"].Split(',').ToList();
            _shipmentServiceCodeList = Configuration["ShipmentServiceCodes"].Split(',').ToList();
            Configuration = configuration;
        }
    }
}
