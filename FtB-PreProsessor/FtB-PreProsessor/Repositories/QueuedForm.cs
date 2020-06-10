using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor
{
    public class QueuedForm
    {
        public string Reference { get; set; }
        public string Status { get; set; }
        public FormContent Content { get; set; }

        public QueuedForm()
        {
            Content = new FormContent();
        }
    }

    public class FormContent
    {
        public string XmlString { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceEditionCode { get; set; }
        public List<AttachmentSummary> AttachmentSummaries { get; set; }
        public List<object> Attachments { get; set; }
    }
}
