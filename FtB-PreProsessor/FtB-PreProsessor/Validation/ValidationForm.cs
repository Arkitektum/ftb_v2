using System.Collections.Generic;

namespace FtB_PreProsessor.Validation
{
    //The structure of the form which the validation API accepts
    public class ValidationForm
    {
        public string DataFormat { get; set; }
        public string DataFormatVersion { get; set; }
        public string Xml { get; set; }
        public List<string> Attachments { get; set; }

    }
}