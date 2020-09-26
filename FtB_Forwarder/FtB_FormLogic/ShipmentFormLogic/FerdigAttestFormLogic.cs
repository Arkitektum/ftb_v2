using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public class FerdigAttestFormLogic : ShipmentFormLogicBase<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public FerdigAttestFormLogic(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }
    }
}
