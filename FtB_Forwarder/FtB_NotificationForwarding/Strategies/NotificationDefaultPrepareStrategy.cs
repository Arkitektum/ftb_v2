﻿using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Strategies
{
    public class NotificationDefaultPrepareStrategy : PrepareStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the NotificationDefaultPrepareStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public NotificationDefaultPrepareStrategy(IForm form) : base(form)
        {

        }
        protected override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for NOTIFICATION");
        }

        public override void Exceute()
        {
            throw new NotImplementedException();
        }

        protected override void ReadReceiverInformation(string archiveReference)
        {
            Console.WriteLine("Leser mottakerinformasjon for NOTIFICATION");
        }

        public void TransformSubmittalToForwardingMessage()
        {
            Console.WriteLine("Transformerer innsending til (antall) mottakere for NOTIFICATION");
        }

    }
}
