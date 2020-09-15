using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Strategies
{
    public class DefaultDistributionPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        private readonly string _archiveReference;
        public DefaultDistributionPrepareStrategy(IForm form) : base(form)
        {
            //_archiveReference = form.
        }
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for DISTRIBUTION");
        }

        public override void Exceute()
        {
            ReadReceiverInformation("archiveReference");
            base.CommonFunction();
            _formBeingProcessed.ProcessPrepareStep();
            base.ReadFromSubmittalQueue("st");
        }

        protected override void ReadReceiverInformation(string archiveReference)
        {
            
            Console.WriteLine("Leser mottakerinformasjon for DISTRIBUTION");
        }

        protected void TransformSubmittalToForwardingMessage()
        {
            Console.WriteLine("Transformerer innsending til (antall) mottakere for DISTRIBUTION");
        }

        public void DoSomething()
        {

        }
    }
}
