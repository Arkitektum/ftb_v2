using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.FormDataRepositories
{
    public class FormDataRepository : IFormDataRepo
    {
        private readonly IBlobOperations _blobOperations;
        public FormDataRepository(IBlobOperations blobOperations)
        {
            _blobOperations = blobOperations;
        }

        public async Task<string> GetFormData(string archiveReference)
        {
            return await _blobOperations.GetFormdata(archiveReference);
        }

        public async Task AddBytesAsBlob(string containerName, string fileName, byte[] fileBytes, IEnumerable<KeyValuePair<string, string>> metadata = null)
        {
            await _blobOperations.AddByteStreamToBlobStorage( Enums.BlobStorageEnum.Private, containerName, fileName, fileBytes, null, metadata);
        }


    }
}
