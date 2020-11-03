using Ftb_DbModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Ftb_Repositories.HttpClients
{
    public class LogEntryHttpClient
    {
        public HttpClient Client { get; }
        private readonly IOptions<FormProcessAPISettings> _settings;

        public LogEntryHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings)
        {
            Client = httpClient;
            _settings = settings;
        }

        public void Post(string archiveReference, IEnumerable<LogEntry> logEntries)
        {
            //api/formprocess/{archiveReference}/logentries

            var requestUri = $"{archiveReference}/logentries";
            var json = JsonSerializer.Serialize(logEntries);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Client.BaseAddress = new Uri(_settings.Value.Uri);
            var result = Client.PostAsync(requestUri, stringContent).GetAwaiter().GetResult();
        }
    }
}
