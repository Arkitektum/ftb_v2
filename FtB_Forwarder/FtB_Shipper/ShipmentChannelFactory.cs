using FtB_Common;
using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_ShipmentForwarding.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentChannelFactory : AbstractChannelFactory
    {
        public override IStrategy CreatePrepareStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new DefaultShipmentPrepareStrategy(form));
        }

        public override IStrategy CreateSendStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new DefaultShipmentSendStrategy(form));
        }

        public override IStrategy CreateReportStrategy(IForm form)
        {
            return (form.GetPrepareStrategy() != null
                ? form.GetPrepareStrategy() : new DefaultShipmentReportStrategy(form));
        }


    }
}
