using FtB_Common.Factories;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public class PrepareStrategyRunner
    {
        private IStrategy _prepareStrategy;
        public PrepareStrategyRunner(AbstractChannelFactory channel, IForm form)
        {
            _prepareStrategy = channel.CreatePrepareStrategy(form);
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
