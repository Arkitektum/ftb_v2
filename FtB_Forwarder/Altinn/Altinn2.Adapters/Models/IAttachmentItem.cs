namespace Altinn2.Adapters.Models
{
    public interface IAttachmentItem
    {
        string ArchiveReference { get; set; }
        byte[] AttachmentData { get; set; }
        int AttachmentId { get; set; }
        string AttachmentType { get; set; }
        string AttachmentTypeName { get; set; }
        string AttachmentTypeNameLanguage { get; set; }
        string FileName { get; set; }
        bool IsEncrypted { get; set; }
    }
}
