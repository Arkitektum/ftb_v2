using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor.Validation
{
    public class ValidationFormMapper : IValdiationFormMapper
    {
        public ValidationForm MapFrom(QueuedForm queuedForm)
        {
            return new ValidationForm(); //{ Xml = queuedForm.Content.XmlString, Attachments = queuedForm.Content.Attachments };
        }
    }
}
