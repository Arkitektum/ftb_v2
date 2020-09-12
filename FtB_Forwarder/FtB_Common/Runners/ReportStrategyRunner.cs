using FtB_Common.Factories;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public class ReportStrategyRunner
    {
        private IStrategy _reportStrategy;
        public ReportStrategyRunner(AbstractChannelFactory channel, IForm form)
        {
            _reportStrategy = channel.CreateReportStrategy(form);
        }

        public void PrepareFormForForwarding()
        {
            _reportStrategy.Exceute();
        }
    }
}
