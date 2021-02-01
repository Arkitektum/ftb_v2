namespace Altinn2.Adapters.Models
{
    public class AttachmentItem : IAttachmentItem
    {
        public string ArchiveReference { get; set; }
        public byte[] AttachmentData { get; set; }
        public string AttachmentType { get; set; }
        public string FileName { get; set; }
        public bool IsEncrypted { get; set; }
        public int AttachmentId { get; set; }
        public string AttachmentTypeName { get; set; }
        public string AttachmentTypeNameLanguage { get; set; }

        public AttachmentItem(string archiveReference, byte[] attachmentData, string attachmentType, string fileName, bool isEncrypted, int attachmentId, string attachmentTypeName, string attachmentTypeNameLanguage)
        {
            ArchiveReference = archiveReference;
            AttachmentData = attachmentData;
            AttachmentType = attachmentType;
            FileName = fileName;
            IsEncrypted = isEncrypted;
            AttachmentId = attachmentId;
            AttachmentTypeName = attachmentTypeName;
            AttachmentTypeNameLanguage = attachmentTypeNameLanguage;
        }
    }
}
