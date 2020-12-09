using FtB_Common.Interfaces;
using System;

namespace FtB_FormLogic
{
    public class OLD_SvarPaaNabovarselFormLogic : OLD_NotificationFormLogicBase<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public OLD_SvarPaaNabovarselFormLogic(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }
    }
}
