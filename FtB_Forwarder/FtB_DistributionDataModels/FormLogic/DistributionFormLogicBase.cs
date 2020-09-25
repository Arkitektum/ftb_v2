using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionFormLogic.FormLogic
{
    public abstract class DistributionFormLogicBase<T> : FormLogicBase<T>
    {
        public DistributionFormLogicBase(IFormDataRepo repo) : base(repo)
        {

        }
        public override void InitiateForm()
        { }


    }
}
