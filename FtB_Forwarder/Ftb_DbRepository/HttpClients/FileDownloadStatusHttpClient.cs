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
    public class FileDownloadStatusHttpClient
    {
        public HttpClient Client { get; }
        private readonly ILogger _log;

        public FileDownloadStatusHttpClient(HttpClient httpClient, IOptions<FormProcessAPISettings> settings, ILogger<DistributionFormsHttpClient> log)
        {
            _log = log;
            _log.LogDebug($"FileDownloadStatusHttpClient - constructor.");
            Client = httpClient;
            Client.BaseAddress = new Uri(settings.Value.Uri);
            Client.DefaultRequestHeaders.Authorization = BasicAuthenticationHelper.GetAuthenticationHeader(settings.Value);
        }

        public async Task<bool> Post(string archiveReference, FileDownloadStatus fileDownload)
        {
            _log.LogDebug($"Post(string, IEn(FileDownloadStatus) for archiveReference {archiveReference}.");

            var requestUri = $"{archiveReference}/filedownloads";
            var json = JsonSerializer.Serialize(fileDownload);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var returnCode = await Client.PostAsync(requestUri, stringContent);
            
            return returnCode.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<FileDownloadStatus>> GetAll(string archiveReference)
        {
            _log.LogDebug($"Get(Guid) FileDownload for id {archiveReference}.");

            if (archiveReference == null)
            {
                throw new ArgumentNullException("Id cannot be null");
            }

            var requestUri = $"{archiveReference}/filedownloads";

            var result = await Client.GetAsync(requestUri);

            IEnumerable<FileDownloadStatus> retVal = null;
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                retVal = JsonSerializer.Deserialize<IEnumerable<FileDownloadStatus>>(content);
            }
            _log.LogDebug($"Returning list of FileDownloadStatus for id {archiveReference}.");

            return retVal;
        }
    }
}
