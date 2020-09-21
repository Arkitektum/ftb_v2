using FtB_Common;
using FtB_Common.Interfaces;

namespace FtB_Reporter
{
    public abstract class ReportStrategyBase : StrategyBase, IStrategy
    {
        public ReportStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
        }
        public abstract void Exceute();
    }
}