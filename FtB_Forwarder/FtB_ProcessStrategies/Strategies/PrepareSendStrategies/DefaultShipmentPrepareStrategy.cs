using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System.Collections.Generic;

namespace FtB_ProcessStrategies
{
    public class DefaultShipmentPrepareStrategy : PrepareStrategyBase
    {
        public DefaultShipmentPrepareStrategy(IFormLogic formLogic, ITableStorage tableStorage) : base(formLogic, tableStorage) { }

        public override List<SendQueueItem> Exceute(SubmittalQueueItem submittalQueueItem)
        {
            FormLogicBeingProcessed.ProcessPrepareStep();
            return null;
        }
    }
}
