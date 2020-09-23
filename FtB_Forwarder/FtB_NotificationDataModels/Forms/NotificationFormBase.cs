using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationDataModels.Forms
{
    public class NotificationFormBase<T> : FormBase<T>
    {
        public NotificationFormBase(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public override void InitiateForm()
        {
            throw new NotImplementedException();
        }
    }
}
