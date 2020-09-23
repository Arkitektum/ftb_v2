using AltinnWebServices.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AltinnWebServices.WS.AltinnDownloadQueue
{
    public class AltinnDownloadQueueClient : IAltinnDownloadQueueClient
    {
        private DownloadQueueExternalBasicClient _client;
        private readonly IOptions<AltinnDownloadQueueConnectionSettings> _connectionOptions;

        public AltinnDownloadQueueClient(Binding binding, IOptions<AltinnDownloadQueueConnectionSettings> connectionOptions)
        {
            _client = new DownloadQueueExternalBasicClient(binding, new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _connectionOptions = connectionOptions;
        }
        public List<IArchivedItemMetadata> GetEnqueuedItems(string serviceCode)
        {
            try
            {
                var result = _client.GetDownloadQueueItemsAsync(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, serviceCode).Result;

                return AltinnDownloadQueueMapper.MapDowloadQueueItem(result);
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

        public AltinnArchivedItem GetArchivedItem(string archiveReference)
        {
            try
            {
                var result = _client.GetArchivedFormTaskBasicDQAsync(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, archiveReference, 1044, false).Result;

                return AltinnDownloadQueueMapper.MapArchivedForm(result);
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

        public void PurgeItemFromQueue(string archiveReference)
        {
            //TODO: Decide if it should return a status of some sort
            try
            {
                var result = _client.PurgeItemAsync(_connectionOptions.Value.UserName, _connectionOptions.Value.Password, archiveReference).Result;
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
