﻿using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Forms
{
    public class NotificationFormBase<T> : FormBase<T>
    {
        public NotificationFormBase(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }
        public override IStrategy GetCustomizedPrepareStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedReportStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedSendStrategy()
        {
            throw new NotImplementedException();
        }

        public override void InitiateForm(string formDataAsXml)
        {
            throw new NotImplementedException();
        }
    }
}
