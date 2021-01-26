namespace FtB_Common.BusinessModels
{
    public class Actor
    {
        public ActorType Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public string PresentationId
        {
            get
            {
                if (Id.Length > 11)
                {
                    //Most likely an encrypted SSN
                    return "anonymous actor";
                }
                return Id;
            }
        }
    }

    public class ActorInternal
    {
        public ActorType Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DecryptedId { get; set; }

        public ActorInternal(Actor actor)
        {
            Id = actor.Id;
            Type = actor.Type;
        }
    }
}
