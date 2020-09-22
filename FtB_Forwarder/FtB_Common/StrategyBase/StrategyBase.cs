using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public abstract class StrategyBase
    {
        //TODO: Er _formBeingProcessed nødvendig å ha her? Eller er PrepareStrategyBase tilstrekkelig nivå? Mao: trenger vi _formBeingProcessed under "Sending"?
        protected IForm _formBeingProcessed;
    }
}
