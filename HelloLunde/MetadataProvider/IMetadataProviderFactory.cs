using MetadataProvider.Providers;

namespace MetadataProvider
{
    public interface IMetadataProviderFactory
    {
        IMetadataProvider GetProvider();
    }
}