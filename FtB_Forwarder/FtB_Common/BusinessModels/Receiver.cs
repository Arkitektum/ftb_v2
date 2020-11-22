namespace FtB_Common.BusinessModels
{
    public class Receiver
    {
        public ReceiverType Type { get; set; }
        public string Id { get; set; }

        public string PresentationId
        {
            get
            {
                if (Id.Length > 11)
                {
                    //Most likely an encrypted SSN
                    return "anonymous receiver";
                }
                return Id;
            }
        }
    }

    public class ReceiverInternal
    {
        public ReceiverType Type { get; set; }
        public string Id { get; set; }
        public string DecryptedId { get; set; }

        public ReceiverInternal(Receiver receiver)
        {
            Id = receiver.Id;
            Type = receiver.Type;
        }
    }
}
