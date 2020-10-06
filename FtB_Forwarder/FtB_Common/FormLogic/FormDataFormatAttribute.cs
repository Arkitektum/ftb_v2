using System;

namespace FtB_Common.FormLogic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormDataFormatAttribute : Attribute
    {
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }

        public string ServiceCode { get; set; }
        public FormLogicProcessingContext ProcessingContext { get; set; } 
    }

    public enum FormLogicProcessingContext
    {
        Prepare,
        Send,
        Report
    }
}
