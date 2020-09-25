using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
namespace FtB_Common
{
    public abstract class StrategyManagerBase
    {
        protected List<string> DistributionServiceCodeList = new List<string>();
        protected List<string> NotificationServiceCodeList = new List<string>();
        protected List<string> ShipmentServiceCodeList = new List<string>();

        public StrategyManagerBase(IConfiguration configuration)
        {
            DistributionServiceCodeList = configuration["DistributionServiceCodes"].Split(',').ToList();
            NotificationServiceCodeList = configuration["NotificationServiceCodes"].Split(',').ToList();
            ShipmentServiceCodeList = configuration["ShipmentServiceCodes"].Split(',').ToList();
        }
    }
}
