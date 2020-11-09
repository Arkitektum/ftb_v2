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
        private readonly Altinn3HttpClient httpClient;

        public PrefillAdapter(ILogger<PrefillAdapter> logger, Altinn3HttpClient httpClient)
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

                    Instance instanceResult = httpClient.PostPrefilledInstance(content, "nabovarsel-plan");                    

                    if (instanceResult == null)
                    {
                        prefillResult.Step = DistriutionStep.Failed;                        
                    }
                    else
                    {                        
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
