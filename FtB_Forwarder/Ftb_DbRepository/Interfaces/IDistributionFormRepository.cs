using Ftb_DbModels;
using System;
using System.Collections.Generic;

namespace Ftb_Repositories.Interfaces
{
    public interface IDistributionFormRepository
    {
        void Add(DistributionForm distributionForm);
        IEnumerable<DistributionForm> Get();
        IEnumerable<DistributionForm> GetWithChildren(Guid distributionReference);
        void SetArchiveReference(string archiveReference);
        void Save();
    }
}