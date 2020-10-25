using Ftb_DbModels;
using System.Collections.Generic;

namespace Ftb_Repositories.Interfaces
{
    public interface IDistributionFormRepository
    {
        void Add(DistributionForm distributionForm);
        IEnumerable<DistributionForm> Get();
        void SetArchiveReference(string archiveReference);
        void Save();
    }
}