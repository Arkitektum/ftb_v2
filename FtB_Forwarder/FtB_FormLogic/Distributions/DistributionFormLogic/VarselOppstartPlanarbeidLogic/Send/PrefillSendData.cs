using FtB_Common.Interfaces;
using FtB_Common.Utils;

namespace FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Send
{
    public abstract class PrefillSendData<T> : IPrefillData
    {
        public T FormInstance { get; private set; }
        public PrefillSendData(T formInstance)
        {
            FormInstance = formInstance;
        }
        public virtual string DataFormatId => PropertyValueExtractor.GetDataFormatId(FormInstance);

        public virtual string DataFormatVersion => PropertyValueExtractor.GetDataFormatVersion(FormInstance);

        public abstract string PrefillFormName { get; }

        public abstract string InitialExternalSystemReference { get; set; }

        public abstract string ExternalSystemReference { get; }

        public abstract string PrefillServiceCode { get; }

        public abstract string PrefillServiceEditionCode { get; }

        /// <summary>
        /// Serializes the content of the FormInstance property
        /// </summary>
        /// <returns>Serialized value of FormInstance</returns>
        public override string ToString()
        {
            return SerializeUtil.Serialize(FormInstance);
        }
    }
}
