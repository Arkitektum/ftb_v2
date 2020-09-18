using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.FormDataRepositories
{
    public class FormDataRepository : IFormDataRepo
    {
        private readonly IBlobOperations _blobOperations;
        public FormDataRepository(IBlobOperations blobOperations)
        {
            _blobOperations = blobOperations;
        }


        public string GetFormData(string archiveReference)
        {
            return _blobOperations.GetFormdata(archiveReference);
        }
    }
}
