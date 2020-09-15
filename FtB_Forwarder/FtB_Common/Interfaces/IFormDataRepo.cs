using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    //public interface IFormDataRepo<T>
    public interface IFormDataRepo
    {
        //T GetFormData(string archiveReference);
        void GetFormData(string archiveReference);
    }
}
