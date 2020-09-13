using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Strategies
{
    public class DefaultShipmentReportStrategy : ReportStrategyBase
    {
        private IForm form;

        public DefaultShipmentReportStrategy(IForm form) : base(form)
        {
            this.form = form;
        }

        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the ShipmentDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public override void Exceute()
        {
            _formBeingProcessed.ProcessCustomReportStep();
        }
    }
}
