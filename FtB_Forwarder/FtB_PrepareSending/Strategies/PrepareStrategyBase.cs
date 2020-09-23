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

        public PrepareStrategyBase(IFormLogic formLogic) : base(formLogic)
        {
        }

        protected abstract void CreateSubmittalDatabaseStatus(string archiveReference);

        protected void ExampleCommonFunction()
        {
            Console.WriteLine("Felles funksjonalitet for både DISTRIBUTION, NOTIFICATION og SHIPMENT");
        }

        public virtual List<SendQueueItem> Exceute()
        {
            ExampleCommonFunction();
            FormLogicBeingProcessed.InitiateForm();
            FormLogicBeingProcessed.ProcessPrepareStep();
            SetReceivers();
            return null;
        }
        private void SetReceivers()
        {
            foreach (var receiver in FormLogicBeingProcessed.Receivers)
            {
                if (!Receivers.Contains(receiver)) //Remove duplicate receivers
                {
                    Receivers.Add(receiver);
                }
                
            }
        }
    }
}