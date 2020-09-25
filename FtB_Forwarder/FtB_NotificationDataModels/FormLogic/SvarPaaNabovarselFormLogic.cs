using FtB_Common.Interfaces;
using System;

namespace FtB_NotificationFormLogic.FormLogic
{
    public class SvarPaaNabovarselFormLogic : NotificationFormLogicBase<no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public SvarPaaNabovarselFormLogic(IFormDataRepo dataRepo) : base(dataRepo)
        {

        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }
    }
}
