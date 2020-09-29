using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class DefaultShipmentPrepareStrategy : PrepareStrategyBase
    {
        public DefaultShipmentPrepareStrategy(ITableStorage tableStorage, ILogger<DefaultShipmentPrepareStrategy> log) : base(tableStorage, log) { }

        public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            FormLogicBeingProcessed.ProcessPrepareStep();
            return null;
        }
    }
}
