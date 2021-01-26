using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{   
    public interface IFormMapper<TFrom>
    {
        //string FormDataString { get; set; }
        IEnumerable<IPrefillData> Map(TFrom from, string receiverId, string receiverName);
    }
}
