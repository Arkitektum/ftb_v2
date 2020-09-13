using FtB_Common.Interfaces;
using System;

namespace FtB_Common
{
    public abstract class PrepareStrategyBase : StrategyBase, IStrategy
    {
        /// <summary>
        /// Scope for this class:
        /// - Public abstract orchestrator methode Execute() 
        /// - Public abstract process steps methodes 
        /// - Protected implementation methods for common functionality for the "Prepare" strategy/process
        /// </summary>

        public PrepareStrategyBase(IForm form)
        {
            _formBeingProcessed = form;
        }
        public abstract void Exceute();
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
    }
}