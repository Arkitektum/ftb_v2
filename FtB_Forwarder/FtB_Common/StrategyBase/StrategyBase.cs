using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public abstract class StrategyBase
    {
        protected IForm _formBeingProcessed;
        protected string _archiveReference;
        protected List<string> _receivers;
        public StrategyBase(IForm form)
        {
            _formBeingProcessed = form;
            _archiveReference = form.ArchiveReference;
            _receivers = form.ReceiverIdentifers;
        }
    }
}
