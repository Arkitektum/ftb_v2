﻿using Ftb_DbModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ftb_Repositories.HttpClients
{
    public class DistributionFormsHttpClient
    {
        public HttpClient Client {get;}
        private readonly IOptions<FormProcessAPISettings> _settings;
        private readonly ILogger _log;

        public DistributionFormsHttpClient(HttpClient httpClient, 
                                           IOptions<FormProcessAPISettings> settings,
                                           ILogger<DistributionFormsHttpClient> log)
        {
            Client = httpClient;
            _settings = settings;
            _log = log;
            Client.BaseAddress = new Uri(_settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            _log.LogDebug($"DistributionFormsHttpClient - constructor.");
        }

        public async Task Post(string archiveReference, IEnumerable<DistributionForm> distributionForms)
        {
            _log.LogDebug($"Post(string, IEn(DistrForm) for archiveReference {archiveReference}.");

            var requestUri = $"{archiveReference}/distributions";
            var json = JsonSerializer.Serialize(distributionForms);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Client.BaseAddress = new Uri(_settings.Value.Uri);
            //Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            var result = await Client.PostAsync(requestUri, stringContent);
        }

        public async Task<IEnumerable<DistributionForm>> Get(string archiveReference)
        {
            _log.LogDebug($"Get(string) distributionForm for archiveReference {archiveReference}.");

            var requestUri = $"{archiveReference}/distributions";

            //Client.BaseAddress = new Uri(_settings.Value.Uri);
            //Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            var result = await Client.GetAsync(requestUri);

            IEnumerable<DistributionForm> retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                retVal = JsonSerializer.Deserialize<IEnumerable<DistributionForm>>(content);
            }
            
            return retVal;
        }

        public async Task<DistributionForm> Get(Guid id)
        {
            _log.LogDebug($"Get(Guid) distributionForm for id {id.ToString()}.");
            
            var requestUri = $"distributions/{id}";

            //Client.BaseAddress = new Uri(_settings.Value.Uri);
            //Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            var result = await Client.GetAsync(requestUri);

            DistributionForm retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                retVal = JsonSerializer.Deserialize<DistributionForm>(content);
            }
            _log.LogDebug($"Returning distributionForm for id {retVal.Id.ToString()}.");

            return retVal;
        }

        public async Task Put(string archiveReference, Guid distributionId, DistributionForm distributionForm)
        {
            _log.LogDebug($"Put (update) distributionForm for archiveReference {archiveReference}.");

            var id = distributionId.ToString().ToUpper();
            var requestUri = $"{archiveReference}/distributions/{id}";
            var json = JsonSerializer.Serialize(distributionForm);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Client.BaseAddress = new Uri(_settings.Value.Uri);
            //Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            var result = await Client.PutAsync(requestUri, stringContent);
            _log.LogDebug($"Put (update) gave result {result.StatusCode.ToString()}.");

        }

    }
}
