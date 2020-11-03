using Ftb_DbModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Ftb_Repositories.HttpClients
{
    public class DistributionFormsHttpClient
    {
        public HttpClient Client {get;}
        private readonly IOptions<FormProcessAPISettings> _settings;
                
        public DistributionFormsHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings)
        {
            Client = httpClient;
            _settings = settings;
        }

        public void Post(string archiveReference, IEnumerable<DistributionForm> distributionForms)
        {
            var requestUri = $"{archiveReference}/distributions";
            var json = JsonSerializer.Serialize(distributionForms);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            var result = Client.PostAsync(requestUri, stringContent).GetAwaiter().GetResult();
        }

        public IEnumerable<DistributionForm> Get(string archiveReference)
        {
            var requestUri = $"{archiveReference}/distributions";

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            var result = Client.GetAsync(requestUri).GetAwaiter().GetResult();

            IEnumerable<DistributionForm> retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                retVal = JsonSerializer.Deserialize<IEnumerable<DistributionForm>>(content);
            }
            return retVal;

        }
    }
}
