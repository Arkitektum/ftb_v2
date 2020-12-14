using Ftb_DbModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ftb_Repositories.Interfaces
{
    public interface IDistributionFormRepository
    {
        void Add(DistributionForm distributionForm);
        Task<IEnumerable<DistributionForm>> Get();
        Task<DistributionForm> Get(Guid id);
        Task Update(string archiveReference, Guid id, DistributionForm updatedDistributionForm);
        Task<IEnumerable<DistributionForm>> GetWithChildren(Guid distributionReference);
        void SetArchiveReference(string archiveReference);
        Task Save();
    }
}