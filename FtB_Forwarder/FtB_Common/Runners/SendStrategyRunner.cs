using FtB_Common.Factories;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public class SendStrategyRunner
    {
        private IStrategy _sendStrategy;
        public SendStrategyRunner(AbstractChannelFactory channel, IForm form)
        {
            _sendStrategy = channel.CreateSendStrategy(form);
        }

        public void PrepareFormForForwarding()
        {
            _sendStrategy.Exceute();
        }
    }
}
