using Altinn.Common.Models;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public class OLD_NotificationFormLogicBase<T> : FormLogicBase<T>
    {
        public OLD_NotificationFormLogicBase(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }
        
        public override void InitiateForm()
        {
            throw new NotImplementedException();
        }
    }
}
