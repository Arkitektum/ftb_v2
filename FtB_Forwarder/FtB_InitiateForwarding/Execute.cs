using FtB_CommonModel.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_InitiateForwarding
{
    public class Execute
    {
        public Execute(string archiveReference)
        {
            Console.WriteLine("I Execute: archiveReference: " + archiveReference);
            //GetFormatId(archiveReference);

            string formatID = GetFormatIdFromForm(archiveReference);
            
            FormFormatIdMapper mapper = new FormFormatIdMapper(formatID);
            Forwarder channelForwarder = new Forwarder(mapper.Channel, mapper.Form);
            channelForwarder.PrepareFormForForwarding();

        }
        private string GetFormatIdFromForm(string archiveReference)
        {

            //Console.WriteLine("GetFormatIdFromForm: formatID=" + formatID);
            return "6325";
        }
        private string GetFormatId(string archiveReference)
        {
            BlobStorage _blobStorage = new BlobStorage(archiveReference);
            string formatID = _blobStorage.GetBlobName(archiveReference);
            return "";
        }
        

    }
}
