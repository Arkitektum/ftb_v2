using FtB_Common.Storage;
using FtB_MessageManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IPrefillService
    {
        string SendPrefill(string archiveReference, string receiver);
    }

    public class PrefillService : IPrefillService
    {
        private readonly IEnumerable<IMessageManager> _messsageManagers;
        private readonly IBlobOperations _blobOperations;

        public PrefillService(IEnumerable<IMessageManager> messsageManagers, IBlobOperations blobOperations)
        {
            _messsageManagers = messsageManagers;
            _blobOperations = blobOperations;
        }

        public string SendPrefill(string archiveReference, string receiver)
        {
            //Get prefill data
            var prefillData = _blobOperations.GetBlobDataByMetadata(archiveReference, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", receiver) });
            foreach (var messageManager in _messsageManagers)
            {
                messageManager.Send(prefillData);
            }

            return prefillData;

        }
    }
}
