using Ftb_DbModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ftb_Repositories.Interfaces
{
    public interface IDistributionFormRepository
    {
        void Add(DistributionForm distributionForm);
        Task<IEnumerable<DistributionForm>> GetAll();
        Task<DistributionForm> Get(string id);
        Task<bool> Update(string archiveReference, Guid id, DistributionForm updatedDistributionForm);
        Task<IEnumerable<DistributionForm>> GetWithChildren(string distributionReference);
        void SetArchiveReference(string archiveReference);
        Task<bool> Save();
    }
}