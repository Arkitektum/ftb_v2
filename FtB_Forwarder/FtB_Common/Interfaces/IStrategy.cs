using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IStrategy<T,U>
    {
        T Exceute(U queueItem);
    }
}
