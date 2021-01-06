using Ftb_DbModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ftb_Repositories.HttpClients
{
    public class LogEntryHttpClient
    {
        public HttpClient Client { get; }

        public LogEntryHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings)
        {
            Client = httpClient;
            Client.BaseAddress = new Uri(settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(settings.Value);
        }

        public async Task Post(string archiveReference, IEnumerable<LogEntry> logEntries)
        {
            var requestUri = $"{archiveReference}/logentries";
            var json = JsonSerializer.Serialize(logEntries);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            await Client.PostAsync(requestUri, stringContent);
        }
    }
}
