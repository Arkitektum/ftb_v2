using FtB_Common;
using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_ShipmentForwarding.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentChannelFactory : ChannelFactoryBase
    {
        public override IStrategy CreatePrepareStrategy(IForm form)
        {
            return (form.GetCustomizedPrepareStrategy() != null
                ? form.GetCustomizedPrepareStrategy() : new DefaultShipmentPrepareStrategy(form));
        }

        public override IStrategy CreateSendStrategy(IForm form)
        {
            return (form.GetCustomizedSendStrategy() != null
                ? form.GetCustomizedSendStrategy() : new DefaultShipmentSendStrategy(form));
        }

        public override IStrategy CreateReportStrategy(IForm form)
        {
            return (form.GetCustomizedReportStrategy() != null
                ? form.GetCustomizedReportStrategy() : new DefaultShipmentReportStrategy(form));
        }


    }
}
