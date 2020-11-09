using Altinn.Platform.Storage.Interface.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;

namespace Altinn3.Adapters
{
    public class Altinn3HttpClient
    {
        public HttpClient Client { get; }
        private readonly IOptions<Altinn3Settings> _options;
        private readonly ILogger<Altinn3HttpClient> _logger;
        private const string _appNamePlaceHolder = "{appname}";
        private const string _ownerIdPlaceHolder = "{owner}";

        public Altinn3HttpClient(HttpClient httpClient, IOptions<Altinn3Settings> options, ILogger<Altinn3HttpClient> logger)
        {
            Client = httpClient;
            _options = options;
            _logger = logger;
        }

        private string GetToken()
        {

            var token = string.Empty;
            //Request token

            //@"http://altinn3local.no/Home/GetTestOrgToken/{owner}";
            var authUri = _options.Value.AuthUrl.ToLower().Replace(_ownerIdPlaceHolder, _options.Value.OwnerUrlIdentifier);
            try
            {
                _logger.LogDebug("Requesting access token");
                HttpResponseMessage response = Client.GetAsync(authUri).Result;
                token = response.Content.ReadAsStringAsync().Result;
                _logger.LogDebug("Access token received");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to request access token");
                throw;
            }

            return token;
        }

        public Instance PostPrefilledInstance(MultipartContent content, string appName)
        {
            var token = GetToken();
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //@"http://altinn3local.no/{owner}/{appname}/instances";
            var requestUri = _options.Value.PlattformUrl.ToLower()
                                .Replace(_appNamePlaceHolder, appName)
                                .Replace(_ownerIdPlaceHolder, _options.Value.OwnerUrlIdentifier);

            _logger.LogDebug("Instantiates prefilled {0}", appName);
            HttpResponseMessage response = Client.PostAsync(requestUri, content).Result;
            string result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Unable to create prefilled instance. Statuscode: {response.StatusCode}, errorMessage: {result}");
                return null;
            }
            else
            {
                _logger.LogDebug("Prefilled {0} instantiated", appName);
                return JsonConvert.DeserializeObject<Instance>(result);
            }                
        }
    }
}
