using FtB_Common.Interfaces;

namespace FtB_Common
{
    public abstract class BaseReportStrategy : IStrategy
    {
        IForm _formBeingProcessed;
        public BaseReportStrategy(IForm form)
        {
            _formBeingProcessed = form;
        }
        public abstract void Exceute();
    }
}