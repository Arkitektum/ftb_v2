﻿using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationDataModels.Forms
{
    public class SvarPaaNabovarselForm : NotificationFormBase<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public SvarPaaNabovarselForm(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }
    }
}
