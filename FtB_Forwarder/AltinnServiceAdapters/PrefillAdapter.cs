using Altinn.Common;
using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using Altinn.Platform.Storage.Interface.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Altinn3.Adapters
{
    public class PrefillAdapter : IPrefillAdapter
    {
        private readonly ILogger<PrefillAdapter> _logger;
        private readonly HttpClient httpClient;

        public PrefillAdapter(ILogger<PrefillAdapter> logger, HttpClient httpClient)
        {
            _logger = logger;
            this.httpClient = httpClient;
        }
        IEnumerable<PrefillResult> IPrefillAdapter.SendPrefill(AltinnDistributionMessage altinnDistributionMessage)
        {
            _logger.LogDebug(@"*               _ _   _               ____   ___  ");
            _logger.LogDebug(@"*         /\   | | | (_)             |___ \ / _ \ ");
            _logger.LogDebug(@"*        /  \  | | |_ _ _ __  _ __     __) | | | |");
            _logger.LogDebug(@"*       / /\ \ | | __| | '_ \| '_ \   |__ <| | | |");
            _logger.LogDebug(@"*      / ____ \| | |_| | | | | | | |  ___) | |_| |");
            _logger.LogDebug(@"*     /_/    \_\_|\__|_|_| |_|_| |_| |____(_)___/ ");

            //Instantiate prefill

            var token = string.Empty;
            //Request token
            var authUri = @"http://altinn3local.no/Home/GetTestOrgToken/oysteinthoensisjord";
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(authUri).Result;
                token = response.Content.ReadAsStringAsync().Result;
            }
            catch (System.Exception)
            {

                throw;
            }

            //Add token
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //Perform instantiation of with prefilled data
            InstanceOwner instanceOwner = new InstanceOwner();
            if (altinnDistributionMessage.NotificationMessage.Receiver.Type == AltinnReceiverType.Privatperson)
                instanceOwner.PersonNumber = altinnDistributionMessage.NotificationMessage.Receiver.Id;
            else
                instanceOwner.OrganisationNumber = altinnDistributionMessage.NotificationMessage.Receiver.Id;

            Instance instanceTemplate = new Instance()
            {
                InstanceOwner = instanceOwner
            };

            var prefillResult = new PrefillResult();

            MultipartFormDataContent content = null;
            byte[] byteArray = Encoding.UTF8.GetBytes(altinnDistributionMessage.PrefilledXmlDataString);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                content = new MultipartContentBuilder(instanceTemplate)
                    .AddDataElement("nabovarselsvarPlanAltinn3", stream, "application/xml")
                    .Build();
                try
                {
                    var requestUri = @"http://altinn3local.no/oysteinthoensisjord/nabovarsel-plan/instances";

                    HttpResponseMessage response = httpClient.PostAsync(requestUri, content).Result;
                    string result = response.Content.ReadAsStringAsync().Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        prefillResult.Step = DistriutionStep.Failed;
                        _logger.LogError($"Unable to create prefilled instance for {altinnDistributionMessage.NotificationMessage.Receiver.Id} - statuscode: {response.StatusCode}, errorMessage: {result}");
                    }
                    else
                    {
                        Instance instanceResult = JsonConvert.DeserializeObject<Instance>(result);
                        prefillResult = new PrefillSentResult() { Step = DistriutionStep.Sent, PrefillReferenceId = instanceResult.SelfLinks.Apps };
                    }
                }
                catch (System.Exception)
                {

                    throw;
                }
            }

            return new List<PrefillResult>() { prefillResult };
        }

        
        
    }
}
