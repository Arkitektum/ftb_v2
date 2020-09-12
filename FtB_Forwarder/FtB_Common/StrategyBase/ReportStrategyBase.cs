using FtB_Common.Interfaces;

namespace FtB_Common
{
    public abstract class ReportStrategyBase : IStrategy
    {
        IForm _formBeingProcessed;
        public ReportStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
        }
        public abstract void Exceute();
    }
}