using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_MessageManager
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageManagerTypeAttribute : Attribute
    {
        public string Id { get; set; }
    }
}
