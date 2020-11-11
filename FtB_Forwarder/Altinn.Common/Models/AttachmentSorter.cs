using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altinn.Common.Models
{
    public class AttachmentSorter
    {

        public IEnumerable<Attachment> GenerateSortedListOfAttachments(IEnumerable<Attachment> attachments)
        {
            AttachmentMetadata[] dok = new AttachmentMetadata[attachments.ToList().Count];
            int dokidx = 0;
            foreach (var att in attachments)
            {
                dok[dokidx] = new AttachmentMetadata
                {
                    //If attachment is not in "vedleggBlankettgruppe", set gruppe to ZZZ to to last in sorted list
                    gruppe = AttachmentSetting.vedleggBlankettgruppe.FirstOrDefault(s => s.Key == att.AttachmentTypeName).Value ?? "ZZZ",
                    attachment = att
                };
                dokidx++;
            }
            
            return dok.OrderBy(b => b.gruppe).Select(b => b.attachment);
        }
    }

    public class AttachmentMetadata
    {
        public string gruppe { get; set; }

        public Attachment attachment { get; set; }
    }
}