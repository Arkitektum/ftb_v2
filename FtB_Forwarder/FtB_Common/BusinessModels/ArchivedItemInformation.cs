using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class ArchivedItemInformation
    {
        public string ArchiveReference { get; set; }
        public string DataFormatID { get; set; }
        public int DataFormatVersionID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceEditionCode { get; set; }
    }
}
