using FtB_Common.Interfaces;
using FtB_DistributionForwarding.Forms;

namespace FtB_DistributionForwarding.Mappers
{
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
