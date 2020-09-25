using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_MessageManager
{
    public interface IMessageManagerFactory
    {
        IMessageManager GetMessageManager();
    }
}
