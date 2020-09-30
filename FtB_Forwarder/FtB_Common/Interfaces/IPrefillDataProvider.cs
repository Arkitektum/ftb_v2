using System;

namespace FtB_Common.Interfaces
{
    public interface IPrefillDataProvider<T>
    {
        public T PrefillFormData { get; set; }
        PrefillData GetPrefillData(string xmlString, string identifier);
    }

    public class PrefillData
    {
        public string ServiceCode { get; set; }
        public string ServiceEditionCode { get; set; }
        public string Reciever { get; set; }
        public string DistributionFormId { get; set; }
        public string ServiceOwnerCode { get; set; }
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }
        public string XmlDataString { get; set; }
        public int DaysValid { get; set; }
        public DateTime? DueDate { get; set; }

        //public string 
    }
}