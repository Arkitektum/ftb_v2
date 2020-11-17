using Altinn2.Adapters.Bindings;
using AltinnWebServices.WS.Correspondence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace Altinn2.Adapters.WS.Correspondence
{
    public class CorrespondenceClient : ICorrespondenceClient
    {
        private CorrespondenceAgencyExternalBasicClient _client;
        private readonly ILogger<CorrespondenceClient> _logger;
        private readonly IOptions<CorrespondenceConnectionSettings> _connectionOptions;

        public CorrespondenceClient(ILogger<CorrespondenceClient> logger, IBindingFactory bindingFactory, IOptions<CorrespondenceConnectionSettings> connectionOptions)
        {
            _client = new CorrespondenceAgencyExternalBasicClient(bindingFactory.GetBindingFor(BindingType.Mtom), new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _logger = logger;
            _connectionOptions = connectionOptions;
        }

        public ReceiptExternal SendCorrespondence(InsertCorrespondenceV2 correspondenceItem, string externalShipmentReference)
        {
            try
            {
                var taskResult = _client.InsertCorrespondenceBasicV2Async(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, _connectionOptions.Value.ServiceOwnerCode, externalShipmentReference, correspondenceItem);
                var result = taskResult.GetAwaiter().GetResult();
                return result.Body.InsertCorrespondenceBasicV2Result;

            }
            catch (FaultException<AltinnFault> af)
            {
                _logger.LogError(af, af.Detail.ToStringExtended());
                _client.Abort();
                throw;
            }
            catch (TimeoutException te)
            {
                _logger.LogError(te, $"Timeout when communicating with Altinn 2 correspondence service");
                _client.Abort();
                throw;
            }
            catch (CommunicationException ce)
            {
                _logger.LogError(ce, $"Communication error occurred when communication with Altinn 2 correspondence service");
                _client.Abort();
                throw;
            }
        }
    }
}
