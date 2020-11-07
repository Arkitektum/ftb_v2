using Ftb_DbModels;
using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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
            if (_distributionForms.Count == 0)
                foreach (var item in _distributionFormsClient.Get(_archiveReference))
                    _distributionForms.Add(item);

            return _distributionForms;
        }

        public IEnumerable<DistributionForm> GetWithChildren(Guid id)
        {
            if (_distributionForms.Count == 0)
                foreach (var item in _distributionFormsClient.Get(_archiveReference))
                    _distributionForms.Add(item);

            return _distributionForms.Where(d => d.Id == id || d.DistributionReference == id);
        }

        public void Add(DistributionForm distributionForm)
        {
            if (_distributionForms.Where(d => d.Id == distributionForm.Id).Count() > 0)
                throw new ArgumentException($"Distribution form {distributionForm.Id} already exist");

            distributionForm.InitialArchiveReference = _archiveReference;
            _distributionForms.Add(distributionForm);
        }

        public void Save()
        {
            if (_distributionForms?.Count > 0)
            {
                SynchronizeDistributionData();
                _logger.LogInformation("Persists distribution forms");
                _distributionFormsClient.Post(_archiveReference, _distributionForms);
            }
            else
                _logger.LogWarning("No forms to persist!");
        }

        private void SynchronizeDistributionData()
        {
            _logger.LogInformation("Synchronizes main distribution form data with combined distribution forms");
            foreach (var item in _distributionForms.Where(d => d.DistributionReference == Guid.Empty))
            {
                var children = _distributionForms.Where(d => d.DistributionReference == item.Id);
                if(children.Count() > 0)
                {
                    foreach(var child in children)
                    {
                        child.DistributionStatus = item.DistributionStatus;
                        child.DistributionType = item.DistributionType;
                        child.SubmitAndInstantiatePrefilled = item.SubmitAndInstantiatePrefilled;
                        child.SubmitAndInstantiatePrefilledFormTaskReceiptId = item.SubmitAndInstantiatePrefilledFormTaskReceiptId;
                        child.SignedArchiveReference = item.SignedArchiveReference;
                        child.Signed = item.Signed;
                        child.DistributionStatus = item.DistributionStatus;
                        child.RecieptSentArchiveReference = item.RecieptSentArchiveReference;
                        child.RecieptSent = item.RecieptSent;
                        child.ErrorMessage = item.ErrorMessage;
                        child.InitialExternalSystemReference = item.InitialExternalSystemReference;
                        child.Printed = item.Printed;
                    }
                }
            }
        }
    }
}
