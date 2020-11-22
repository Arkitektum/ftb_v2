using Ftb_DbModels;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ftb_Repositories.HttpClients
{
    public class FormMetadataHttpClient
    {
        public HttpClient Client { get; }
        
        private readonly IOptions<FormProcessAPISettings> _settings;

        public FormMetadataHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings)
        {
            Client = httpClient;
            _settings = settings;
        }

        public async Task Update(FormMetadata formMetadata)
        {
            var requestUri = $"{formMetadata.ArchiveReference}/formmetadata";
            var json = JsonSerializer.Serialize(formMetadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            var result = await Client.PostAsync(requestUri, stringContent);
        }

        public async Task< FormMetadata> Get(string archiveReference)
        {
            var requestUri = $"{archiveReference}/formmetadata";

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(_settings.Value);
            var result = await Client.GetAsync(requestUri);

            FormMetadata retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                retVal = JsonSerializer.Deserialize<FormMetadata>(content);
            }
            return retVal;

        }
    }
}
