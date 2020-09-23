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
        public DefaultDistributionPrepareStrategy(IForm form) : base(form) { }
        
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for DISTRIBUTION");
        }

        public override List<SendQueueItem> Exceute()
        {
            ReadReceiverInformation("archiveReference");
            base.CommonFunction();
            _formBeingProcessed.InitiateForm();
            _formBeingProcessed.ProcessPrepareStep(); // Må returnere List<SendQueueItem> 
            base.ReadFromSubmittalQueue("st");

            List<SendQueueItem> liste = new List<SendQueueItem>();
            int nummer = 1000; //Used for testing
            foreach (var receiver in _formBeingProcessed.ReceiverIdentifers)
            {
                liste.Add(new SendQueueItem() { ArchiveReference = _archiveReference, PrefillId = "Pref"+ nummer.ToString(), ReceiverName = receiver });
                nummer++; //Used for testing
            }
            return liste;
            
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
