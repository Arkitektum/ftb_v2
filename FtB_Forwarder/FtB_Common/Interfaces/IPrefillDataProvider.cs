using Altinn.Common.Models;

namespace FtB_Common.Interfaces
{
    /// <summary>
    /// Creates the distribution message which will be sent to the altinn interface
    /// </summary>
    /// <typeparam name="T">Prefill datatype</typeparam>
    /// <typeparam name="TB">Main form datatype</typeparam>
    public interface IDistributionDataMapper<T, TB>
    {
        public T PrefillFormData { get; set; }
        AltinnDistributionMessage GetDistributionMessage(string prefillXmlString, TB mainFormData, string distributionFormId);
    }
}