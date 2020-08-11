using Ftb_DbModels;

namespace Ftb_DbRepository
{
    public interface IFormMetadataRepository
    {
        FormMetadata GetByReference(string archiveReference);
    }
}