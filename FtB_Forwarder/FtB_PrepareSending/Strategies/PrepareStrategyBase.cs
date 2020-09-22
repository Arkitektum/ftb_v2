using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;

namespace FtB_PrepareSending.Strategies
{
    public abstract class PrepareStrategyBase : StrategyBase, IStrategy<SendQueueItem>
    {
        /// <summary>
        /// Scope for this class:
        /// - Public abstract orchestrator methode Execute() 
        /// - Public abstract process steps methodes 
        /// - Protected implementation methods for common functionality for the "Prepare" strategy/process
        /// </summary>
        protected string _archiveReference;

        public PrepareStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
            _archiveReference = form.ArchiveReference;
        }
        protected abstract void ReadReceiverInformation(string archiveReference);
        protected abstract void CreateSubmittalDatabaseStatus(string archiveReference);
        protected void ReadFromSubmittalQueue(string archiveReference)
        {
            Console.WriteLine("Fellesmetode: Leser fra innsendingskø for både DISTRIBUTION, NOTIFICATION og SHIPMENT");
        }
        protected void CommonFunction()
        {
            Console.WriteLine("Felles funksjonalitet for både DISTRIBUTION, NOTIFICATION og SHIPMENT");
        }

        public abstract List<SendQueueItem> Exceute();
    }
}