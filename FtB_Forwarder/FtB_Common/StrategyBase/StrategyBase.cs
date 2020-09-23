using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public abstract class StrategyBase
    {
        protected IFormLogic FormLogicBeingProcessed;
        protected string ArchiveReference;
        protected List<Receiver> Receivers;
        public StrategyBase(IFormLogic formLogic)
        {
            FormLogicBeingProcessed = formLogic;
            ArchiveReference = formLogic.ArchiveReference;
            Receivers = formLogic.Receivers;
        }
    }
}
