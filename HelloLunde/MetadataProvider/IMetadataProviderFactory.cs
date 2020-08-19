using MetadataProvider.Providers;

namespace MetadataProvider
{
    public interface IMetadataProviderFactory
    {
        IComicProvider GetProvider();
    }
}