using FtB_Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface ITableStorageOperations
    {
        ReceiverStatusLogEnum GetReceiverLastProcessStatus(string partitionKey);
    }
}
