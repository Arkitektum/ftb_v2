using Ftb_DbModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Ftb_Repositories.HttpClients
{
    public class FormMetadataHttpClient
    {
        //public HttpClient Client { get; }
        private readonly HttpClient Client;
        private readonly IOptions<FormProcessAPISettings> _settings;

        //public FormMetadataHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings)
        public FormMetadataHttpClient(IHttpClientFactory httpClientFacotry, IOptions<FormProcessAPISettings> settings)
        {
            Client = httpClientFacotry.CreateClient("FormMetadataHttpClient");
            _settings = settings;
        }

        public void Update(FormMetadata formMetadata)
        {
            var requestUri = $"{formMetadata.ArchiveReference}/formmetadata";
            var json = JsonSerializer.Serialize(formMetadata);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            var result = Client.PostAsync(requestUri, stringContent).GetAwaiter().GetResult();
        }

        public FormMetadata Get(string archiveReference)
        {
            var requestUri = $"{archiveReference}/formmetadata";

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            var result = Client.GetAsync(requestUri).GetAwaiter().GetResult();

            FormMetadata retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                retVal = JsonSerializer.Deserialize<FormMetadata>(content);
            }
            return retVal;

        }
    }
}
