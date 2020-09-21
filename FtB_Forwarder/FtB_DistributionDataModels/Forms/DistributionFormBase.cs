using FtB_Common.Forms;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionDataModels.Forms
{
    //public abstract class DistributionFormBase<T> : FormBase<T>
    public abstract class DistributionFormBase<T> : FormBase<T>
    {        
        public DistributionFormBase(IFormDataRepo repo) : base(repo)
        {

        }
    }
}
