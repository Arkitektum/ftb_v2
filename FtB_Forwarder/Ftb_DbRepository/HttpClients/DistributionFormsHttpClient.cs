using Ftb_DbModels;
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
        private readonly ILogger _log;

        public DistributionFormsHttpClient(HttpClient httpClient, 
                                           IOptions<FormProcessAPISettings> settings,
                                           ILogger<DistributionFormsHttpClient> log)
        {
            Client = httpClient;
            _log = log;
            Client.BaseAddress = new Uri(settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(settings.Value);
            _log.LogDebug($"DistributionFormsHttpClient - constructor.");
        }

        public async Task<HttpResponseMessage> Post(string archiveReference, IEnumerable<DistributionForm> distributionForms)
        {
            _log.LogDebug($"Post(string, IEn(DistrForm) for archiveReference {archiveReference}.");

            var requestUri = $"{archiveReference}/distributions";
            var json = JsonSerializer.Serialize(distributionForms);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await Client.PostAsync(requestUri, stringContent);
        }

        public async Task<IEnumerable<DistributionForm>> GetAll(string archiveReference)
        {
            _log.LogDebug($"Get(string) distributionForm for archiveReference {archiveReference}.");

            var requestUri = $"{archiveReference}/distributions";

            var result = await Client.GetAsync(requestUri);

            IEnumerable<DistributionForm> retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                retVal = JsonSerializer.Deserialize<IEnumerable<DistributionForm>>(content);
            }
            
            return retVal;
        }

        public async Task<DistributionForm> Get(string id)
        {
            _log.LogDebug($"Get(Guid) distributionForm for id {id}.");

            if (id == null)
            {
                throw new ArgumentNullException("Id cannot be null");
            }

            var requestUri = $"distributions/{id}";

            var result = await Client.GetAsync(requestUri);

            DistributionForm retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                retVal = JsonSerializer.Deserialize<DistributionForm>(content);
            }
            _log.LogDebug($"Returning distributionForm for id {retVal?.Id}.");

            return retVal;
        }

        public async Task<HttpResponseMessage> Put(string archiveReference, Guid distributionId, DistributionForm distributionForm)
        {
            _log.LogDebug($"Put (update) distributionForm for archiveReference {archiveReference}.");

            var id = distributionId.ToString().ToUpper();
            var requestUri = $"{archiveReference}/distributions/{id}";
            var json = JsonSerializer.Serialize(distributionForm);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await Client.PutAsync(requestUri, stringContent);
            _log.LogDebug($"Put (update) gave result {result.StatusCode.ToString()}.");
            return result;
        }
    }
}
