using Ftb_DbModels;
using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<DistributionForm>> GetAll()
        {
            if (_distributionForms.Count == 0)
                foreach (var item in await _distributionFormsClient.GetAll(_archiveReference))
                    _distributionForms.Add(item);

            return _distributionForms;
        }

        public async Task<DistributionForm> Get(string id)
        {
            var result = _distributionForms.Where(d => d.Id.ToString().Equals(id)).FirstOrDefault();
            if (result == null)
            {
                result = await _distributionFormsClient.Get(id);
            }

            return result;
        }

        public async Task<bool> Update(string archiveReference, Guid id, DistributionForm updatedDistributionForm)
        {
            HttpResponseMessage result;
            _logger.LogInformation($"Updates distribution form for archiveReference {archiveReference} with id={id}");
            result = await _distributionFormsClient.Put(archiveReference, id, updatedDistributionForm);
            if (!result.IsSuccessStatusCode)
            {
                _logger.LogError($"Updating distribution form returned httpStatusCode {result.StatusCode.ToString()}");
            }
            return result.IsSuccessStatusCode;
        }


        public async Task<IEnumerable<DistributionForm>> GetWithChildren(string id)
        {
            if (_distributionForms.Count == 0)
                foreach (var item in await _distributionFormsClient.GetAll(_archiveReference))
                    _distributionForms.Add(item);

            return _distributionForms.Where(d => d.Id.ToString().Equals(id) || d.DistributionReference.ToString().Equals(id));
        }

        public void Add(DistributionForm distributionForm)
        {
            if (_distributionForms.Where(d => d.Id == distributionForm.Id).Count() > 0)
                throw new ArgumentException($"Distribution form {distributionForm.Id} already exist");

            distributionForm.InitialArchiveReference = _archiveReference;
            _distributionForms.Add(distributionForm);
        }

        public async Task<bool> Save()
        {
            HttpResponseMessage result;
            if (_distributionForms?.Count > 0)
            {
                SynchronizeDistributionData();
                _logger.LogInformation("Persists distribution forms");
                result = await _distributionFormsClient.Post(_archiveReference, _distributionForms);
                if (!result.IsSuccessStatusCode)
                {
                    _logger.LogError($"Persisting distribution forms returned httpStatusCode {result.StatusCode.ToString()}");
                }
                return result.IsSuccessStatusCode;
            }
            else
            {
                _logger.LogWarning("No forms to persist!");
                return true;
            }
 
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
