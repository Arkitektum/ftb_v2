using Ftb_DbModels;
using System.Threading.Tasks;

namespace Ftb_Repositories.Interfaces
{
    public interface IFormMetadataRepository
    {
        Task<FormMetadata> Get();
        void Update(FormMetadata formMetadata);
        Task Save();
        void SetArchiveReference(string archiveReference);
    }
}