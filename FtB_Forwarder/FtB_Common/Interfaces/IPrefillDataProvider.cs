using Altinn.Common.Models;

namespace FtB_Common.Interfaces
{
    public interface IPrefillDataProvider<T>
    {
        public T PrefillFormData { get; set; }
        PrefillData GetPrefillData(string xmlString, string distributionFormId);
    }
}