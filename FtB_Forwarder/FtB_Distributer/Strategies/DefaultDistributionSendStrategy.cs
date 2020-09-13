    using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Strategies
{
    public class DefaultDistributionSendStrategy : BaseSendStrategy
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultDistributionSendStrategy(IForm form) : base(form)
        {

        }

        public override void Exceute()
        {
            _formBeingProcessed.ProcessCustomSendStep();
        }

        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til DISTRIBUTION");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }
    }
}
