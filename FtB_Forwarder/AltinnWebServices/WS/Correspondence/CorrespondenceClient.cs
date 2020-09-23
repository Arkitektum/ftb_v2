using AltinnWebServices.WS.Correspondence.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace AltinnWebServices.WS.Correspondence
{    
    public class CorrespondenceClient
    {
        private CorrespondenceAgencyExternalBasicClient _client;
        private readonly IOptions<AltinnCorrespondenceConnectionSettings> _connectionOptions;

        public CorrespondenceClient(Binding binding, IOptions<AltinnCorrespondenceConnectionSettings> connectionOptions)
        {
            _client = new CorrespondenceAgencyExternalBasicClient(binding, new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _connectionOptions = connectionOptions;
        }

        public void SendCorrespondence(CorrespondenceItem correspondenceItem)
        {
            try
            {
                //Map to InsertCorrespondenceV2
                InsertCorrespondenceV2 correspondence = CorrespondenceMapper.Map(correspondenceItem);
                var externalShipmentReference = correspondenceItem.ExternalShipmentReference;


                var taskResult = _client.InsertCorrespondenceBasicV2Async(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, _connectionOptions.Value.SystemUserCode, externalShipmentReference, null);

                

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
