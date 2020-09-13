using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Factories
{
    public abstract class ChannelFactoryBase
    {
        public abstract IStrategy CreatePrepareStrategy(IForm form);
        public abstract IStrategy CreateSendStrategy(IForm form);
        public abstract IStrategy CreateReportStrategy(IForm form);
    }
}
