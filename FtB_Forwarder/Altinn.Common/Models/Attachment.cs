namespace Altinn.Common.Models
{
    public class Attachment
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public string SendersReference { get; set; }
        public byte[] Bytes { get; set; }
        public string Url { get; set; }
    }
}
