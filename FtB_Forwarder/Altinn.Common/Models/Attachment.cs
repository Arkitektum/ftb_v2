namespace Altinn.Common.Models
{
    public abstract class Attachment
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SendersReference { get; set; }        
        public string Url { get; set; }        
    }

    public class AttachmentBinary : Attachment
    {
        public byte[] BinaryContent { get; set; }
    }

    public class AttachmentXml : Attachment
    {
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }
        public string XmlStringContent { get; set; }
    }
}
