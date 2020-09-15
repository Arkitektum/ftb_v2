using System;

namespace FtB_Common.Forms
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormDataFormatAttribute : Attribute
    {
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }
    }
}
