using Altinn.Common.Models;
using System;
using System.Collections.Generic;

namespace FtB_Common.Interfaces
{
    /// <summary>
    /// Creates the distribution message which will be sent to the altinn interface
    /// </summary>
    /// <typeparam name="T">Prefill datatype</typeparam>
    /// <typeparam name="TB">Main form datatype</typeparam>
    public interface IDistributionDataMapper<TB>
    {
        //T PrefillFormData { get; set; }
        AltinnDistributionMessage GetDistributionMessage(IEnumerable<IPrefillData> prefillData, TB mainFormData, Guid distributionFormId, string archiveReference);
    }

    //public interface ISendData
    //{
    //    string PrefillFormName { get; }
    //    string ExternalSystemMainReference { get; set; }
    //    string ExternalSystemSubReference { get; }
    //    string PrefillServiceCode { get; }
    //    string PrefillServiceEditionCode { get; }
    //}
}