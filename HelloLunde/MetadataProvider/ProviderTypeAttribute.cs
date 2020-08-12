using System;

namespace MetadataProvider
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProviderTypeAttribute : Attribute
    {
        public string Id { get; set; }
    }
}
