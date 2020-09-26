using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public abstract class PrefillFormMapperBase<TFrom, TTo>
    {
        public abstract TTo Map(TFrom from, string filter);
    }
}
