using Ftb_DbModels;
using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Ftb_Repositories
{
    public class DistributionFormsRepository : IDistributionFormRepository
    {
        private ConcurrentBag<DistributionForm> _distributionForms = new ConcurrentBag<DistributionForm>();
        private string _archiveReference;
        private readonly ILogger<DistributionFormsRepository> _logger;
        private readonly DistributionFormsHttpClient _distributionFormsClient;

        public DistributionFormsRepository(ILogger<DistributionFormsRepository> logger, DistributionFormsHttpClient distributionFormsClient)
        {
            _logger = logger;
            _distributionFormsClient = distributionFormsClient;
        }
        public void SetArchiveReference(string archiveReference)
        {
            _archiveReference = archiveReference;
        }
        public IEnumerable<DistributionForm> Get()
        {
            return _distributionFormsClient.Get(_archiveReference);
        }

        public void Add(DistributionForm distributionForm)
        {
            _distributionForms.Add(distributionForm);
        }

        public void Save()
        {
            if (_distributionForms?.Count > 0)
                _distributionFormsClient.Post(_archiveReference, _distributionForms);
        }
    }
}
