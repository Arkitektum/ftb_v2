using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_MessageManager
{
    public interface IMessageManager
    {
        Task Send(dynamic messageElement);

    }
}
