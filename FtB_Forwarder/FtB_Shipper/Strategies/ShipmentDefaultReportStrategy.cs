using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Strategies
{
    public class ShipmentDefaultReportStrategy : ReportStrategyBase
    {
        private IForm form;

        public ShipmentDefaultReportStrategy(IForm form) : base(form)
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
            throw new NotImplementedException();
        }
    }
}
