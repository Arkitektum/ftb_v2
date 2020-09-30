using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public class ShipmentFormLogicBase<T> : FormLogicBase<T>
    {
        public ShipmentFormLogicBase(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public override PrefillData GetPrefillData(string filter, string identifier)
        {
            throw new NotImplementedException();
        }

        public override void InitiateForm()
        {
            throw new NotImplementedException();
        }
    }
}
