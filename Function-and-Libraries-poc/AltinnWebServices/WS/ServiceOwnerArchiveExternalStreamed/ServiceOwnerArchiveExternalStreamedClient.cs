using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AltinnWebServices.WS.ServiceOwnerArchiveExternalStreamed
{
    public class ServiceOwnerArchiveExternalStreamedClient : IServiceOwnerArchiveExternalStreamedClient
    {
        private readonly IOptions<AltinnServiceOwnerConnectionSettings> _connectionOptions;
        private ServiceOwnerArchiveExternalStreamedBasicClient _client;
        public ServiceOwnerArchiveExternalStreamedClient(Binding binding, IOptions<AltinnServiceOwnerConnectionSettings> connectionOptions)
        {
            _client = new ServiceOwnerArchiveExternalStreamedBasicClient(binding, new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _connectionOptions = connectionOptions;
        }

        public byte[] GetAttachmentData(int attachmentId)
        {
            try
            {
                var attachmentData = _client.GetAttachmentDataStreamedBasicAsync(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, attachmentId).Result.Body.GetAttachmentDataStreamedBasicResult;
                _client.Close();

                return attachmentData;
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

        public void Dispose()
        {
            if (_client.State != CommunicationState.Closed)
                _client.Close();

            _client = null;
        }
    }
}
