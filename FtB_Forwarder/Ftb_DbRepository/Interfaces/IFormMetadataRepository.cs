using Ftb_DbModels;

namespace Ftb_Repositories.Interfaces
{
    public interface IFormMetadataRepository
    {
        FormMetadata Get();
        void Update(FormMetadata formMetadata);
        void Save();
        void SetArchiveReference(string archiveReference);
    }
}