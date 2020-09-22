    using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Sender.Strategies
{
    public class DefaultDistributionSendStrategy : SendStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultSendStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DefaultDistributionSendStrategy(IForm form) : base(form)
        {

        }

        public override List<ReportQueueItem> Exceute()
        {
            _formBeingProcessed.ProcessSendStep();
            return null;
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
