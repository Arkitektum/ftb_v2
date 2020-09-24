using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionFormLogic.FormLogic
{
    public abstract class PrefillFormMapperBase<TFrom, TTo>
    {
        public abstract TTo Map(TFrom from, string filter);
    }
}
