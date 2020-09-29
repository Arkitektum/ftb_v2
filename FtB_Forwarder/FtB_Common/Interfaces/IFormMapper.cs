using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{   
    public interface IFormMapper<TFrom, TTo>
    {
        string FormDataString { get; set; }
        TTo Map(TFrom from, string filter);
    }
}
