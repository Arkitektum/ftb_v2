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
        

        public FormMetadataHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings)
        {
            Client = httpClient;
            Client.BaseAddress = new Uri(settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(settings.Value);
        }

        public async Task Update(FormMetadata formMetadata)
        {
            var requestUri = $"{formMetadata.ArchiveReference}/formmetadata";
            var json = JsonSerializer.Serialize(formMetadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await Client.PutAsync(requestUri, stringContent);
        }

        public async Task< FormMetadata> Get(string archiveReference)
        {
            var requestUri = $"{archiveReference}/formmetadata";
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
