using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_Common.Interfaces
{
    public interface IFormDataRepo
    {
        Task<string> GetFormData(string archiveReference);
        void AddBytesAsBlob(string containerName, string fileName, byte[] fileBytes, IEnumerable<KeyValuePair<string, string>> metadata = null);
    }
}
