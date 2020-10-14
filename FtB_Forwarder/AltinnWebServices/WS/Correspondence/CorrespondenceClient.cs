using Altinn2.Adapters.Bindings;
using AltinnWebServices.WS.Correspondence;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Altinn2.Adapters.WS.Correspondence
{
    public class CorrespondenceClient : ICorrespondenceClient
    {
        private CorrespondenceAgencyExternalBasicClient _client;
        private readonly IOptions<CorrespondenceConnectionSettings> _connectionOptions;

        public CorrespondenceClient(IBindingFactory bindingFactory, IOptions<CorrespondenceConnectionSettings> connectionOptions)
        {
            _client = new CorrespondenceAgencyExternalBasicClient(bindingFactory.GetBindingFor(BindingType.Mtom), new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _connectionOptions = connectionOptions;
        }

        public ReceiptExternal SendCorrespondence(InsertCorrespondenceV2 correspondenceItem)
        {
            try
            {
                var taskResult = _client.InsertCorrespondenceBasicV2Async(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, _connectionOptions.Value.ServiceOwnerCode, correspondenceItem.ArchiveReference, correspondenceItem);
                var result = taskResult.GetAwaiter().GetResult();
                return result.Body.InsertCorrespondenceBasicV2Result;

            }
            catch (TimeoutException)
            {
                _client.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                _client.Abort();
                throw;
            }
        }
    }
}
