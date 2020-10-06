using no.kxml.skjema.dibk.nabovarselsvarPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public interface IFormDataValidator
    {
        bool IsValid();
    }

    public class SvarPaNabovarselPlanValidator : IFormDataValidator
    {
        private readonly SvarPaaNabovarselPlanType _data;

        public SvarPaNabovarselPlanValidator(no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType data)
        {
            _data = data;
        }
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_data.beroertPart.organisasjonsnummer) && !string.IsNullOrWhiteSpace(_data.beroertPart.foedselsnummer);
        }
    }


}
