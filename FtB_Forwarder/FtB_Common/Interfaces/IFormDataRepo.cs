using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IFormDataRepo
    {
        string GetFormData(string archiveReference);
        void AddBytesAsBlob(string containerName, string fileName, byte[] fileBytes, IEnumerable<KeyValuePair<string, string>> metadata = null);
    }
}
