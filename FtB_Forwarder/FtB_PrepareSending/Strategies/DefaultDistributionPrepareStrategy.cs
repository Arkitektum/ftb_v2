using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PrepareSending.Strategies
{
    public class DefaultDistributionPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        private string _archiveReference;
        public DefaultDistributionPrepareStrategy(IForm form) : base(form)
        {
            //_archiveReference = form.
        }
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            _archiveReference = archiveReference;
            Console.WriteLine("Oppretter databasestatus for DISTRIBUTION");
        }

        public override List<SendQueueItem> Exceute()
        {
            ReadReceiverInformation("archiveReference");
            base.CommonFunction();
            _formBeingProcessed.ProcessPrepareStep(); // Må returnere List<SendQueueItem> 
            base.ReadFromSubmittalQueue("st");
            return new List<SendQueueItem>()
            {
                new SendQueueItem(){ArchiveReference = _archiveReference, PrefillId = "Pref1000"},
                new SendQueueItem(){ArchiveReference = _archiveReference, PrefillId = "Pref2000"},
                new SendQueueItem(){ArchiveReference = _archiveReference, PrefillId = "Pref3000"},
                new SendQueueItem(){ArchiveReference = _archiveReference, PrefillId = "Pref4000"}
            };
            
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
