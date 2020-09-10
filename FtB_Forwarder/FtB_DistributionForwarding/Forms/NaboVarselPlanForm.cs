using FtB_CommonModel.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Forms
{
    public class NaboVarselPlanForm : DistributionForm
    {
        public override void Process()
        {
            Console.WriteLine("Spesialhåndtering av skjema for NABOVARSELPLAN");
            this.OptionalMethod();
        }
        public override void OptionalMethod()
        {
            base.OptionalMethod();
            Console.WriteLine("Valgfri metode implementert for skjema NABOVARSELPLAN");
        }
    }
}
