using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Strategies
{
    public class DefaultNotificationSendStrategy : SendStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultNotificationSendStrategy(IForm form) : base(form)
        {

        }

        public override void Exceute()
        {
            _formBeingProcessed.ProcessSendStep();
        }

        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til NOTIFICATION");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for NOTIFICATION");
        }
    }
}
