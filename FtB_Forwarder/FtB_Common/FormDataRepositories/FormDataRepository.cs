using FtB_Common.Interfaces;
using FtB_Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.FormDataRepositories
{
    public class FormDataRepository<T> : IFormDataRepo<T>
    {
        public T GetFormData(string archiveReference)
        {
            //Get data from somewhere
            var data = "fdasfasfasdfdsafasfds";

            return SerializeUtil.DeserializeFromString<T>(data);
        }
    }
}
