using Ftb_DbModels;
using System.Threading.Tasks;

namespace Ftb_Repositories.Interfaces
{
    public interface IFormMetadataRepository
    {
        Task<FormMetadata> Get();
        Task<FormMetadata> Get(string archiveReference);
        void Update(FormMetadata formMetadata);
        Task Save();
        void SetArchiveReference(string archiveReference);
    }
}