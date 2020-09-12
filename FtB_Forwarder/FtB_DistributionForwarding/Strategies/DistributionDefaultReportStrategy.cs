using FtB_Common;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_DistributionForwarding.Strategies

{
    public class DistributionDefaultReportStrategy : ReportStrategyBase
    {
        /// <summary>
        /// Scope for this class:
        /// - Protected methods for common functionality for the DistributionDefaultReportStrategy
        /// - Public orchestrator methode Execute() 
        /// </summary>
        public DistributionDefaultReportStrategy(IForm form) : base(form)
        {

        }
        public override void Exceute()
        {
            throw new NotImplementedException();
        }
    }
}
