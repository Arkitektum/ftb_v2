﻿using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentChannelFactory : AbstractChannelFactory
    {
        public override PrepareBase CreatePrepareBase(FormBase form)
        {
            return new ShipmentPrepareer(form);
        }

        public override SendBase CreateSendBase(FormBase form)
        {
            return new ShipmentSender(form);
        }

        public override Reportbase CreateReportBase()
        {
            return new ShipmentReporter();
        }
    }
}
