using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Mappers
{
    public static class FormatIdToDistributionFormMapper
    {
        public static IForm GetForm(string formatID)
        {
            switch (formatID)
            {
                case "6325":
                    return new NaboVarselPlanForm();
                case "12345":
                    //form = new NokLittNyDistributionForm();
                    return null;
                default:
                    return null;
            }
        }
    }
}
