using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Mappers
{
    public class FormatIdToDistributionFormMapper2
    {
        private readonly IEnumerable<IForm> _forms;

        public FormatIdToDistributionFormMapper2(IEnumerable<IForm> forms)
        {
            _forms = forms;
        }

        public IForm GetForm(string formatID)
        {
            foreach (var form in _forms)
            {
                if (form.GetFormatId().Equals(formatID))
                {
                    return form;
                }
            }
            throw new ArgumentException($"Unrecognized DataFormatId ({formatID}) ", "Error");
        }
    }


    public static class FormatIdToDistributionFormMapper
    {
        public static IForm GetForm(string formatID)
        {
            //Must get the form XML data from BlobStorage
            //This can be used to initiate the form
            switch (formatID)
            {
                case "6303":
                    //return new NaboVarselPlanForm<T> formDataRepo);
                    return new NaboVarselPlanForm();
                case "12345":
                    //return new AnnetSkjemaAvTypeDistributionForm();
                    return null;
                default:
                    return null;
            }
        }
    }
}
