﻿using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Forms
{
    public class FerdigAttestForm : ShipmentFormBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public FerdigAttestForm(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public IStrategy GetCustomizedReportStrategy()
        {
            throw new NotImplementedException();
        }

        public override IStrategy GetCustomizedSendStrategy()
        {
            throw new NotImplementedException();
        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }

        public override void InitiateForm(string formDataAsXml)
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomPrepareStep()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomReportStep()
        {
            throw new NotImplementedException();
        }

        public void ProcessCustomSendStep()
        {
            throw new NotImplementedException();
        }
    }
}
