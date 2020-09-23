using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentDataModels.FormLogic
{
    public class ShipmentFormLogicBase<T> : FormLogicBase<T>
    {
        public ShipmentFormLogicBase(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public override void InitiateForm()
        {
            throw new NotImplementedException();
        }
    }
}
