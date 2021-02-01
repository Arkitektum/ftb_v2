using System;
using System.Collections.Generic;

namespace Altinn2.Adapters.Models
{
    /// <summary>
    /// Abstraction of the ArchivedFormTaskDQBE type
    /// </summary>
    public class AltinnArchivedItem
    {
        //public AltinnWebServices.WS.AltinnDownloadQueue.ApproverListDQBE Approvers; //Not implemented since no references found
        public string ArchiveReference { get; set; }
        public DateTime ArchiveTimeStamp { get; set; }
        public List<IAttachmentItem> Attachments { get; set; }
        public int AttachmentCount { get; set; }
        public int CaseID { get; set; }
        public long CorrelationReference { get; set; }
        public List<IFormItem> Forms { get; set; }
        public int FormsCount { get; set; }
        public string Reportee { get; set; }
        //public AltinnWebServices.WS.AltinnDownloadQueue.SOEncryptedSymmetricKeyExternalDQBE SOEncryptedSymmetricKey { get; set; } //Not implemented since no references found
        public string ServiceCode { get; set; }
        public string ServiceEditionCode { get; set; }

        public AltinnArchivedItem()
        {
            Attachments = new List<IAttachmentItem>();
            Forms = new List<IFormItem>();
        }

    }
}
