using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Strategies
{
    public class DefaultNotificationReportStrategy : BaseReportStrategy
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultNotificationReportStrategy(IForm form) : base(form)
        {

        }
        public override void Exceute()
        {
            _formBeingProcessed.ProcessCustomReportStep();
        }
    }
}
