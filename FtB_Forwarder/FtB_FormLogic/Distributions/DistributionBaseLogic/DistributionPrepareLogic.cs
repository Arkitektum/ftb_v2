﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    public abstract class DistributionPrepareLogic<T> : PrepareLogic<T>
    {
        private readonly ILogger log;
        protected  override  List<Receiver> Receivers { get => base.Receivers; set => base.Receivers = value; }

        public DistributionPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log) : base(repo, tableStorage, log)
        {
            this.log = log;
        }

        public override IEnumerable<SendQueueItem> Execute(SubmittalQueueItem submittalQueueItem)
        {
            log.LogInformation("Jau");
            return base.Execute(submittalQueueItem);
        }

        protected override  abstract void GetReceivers();
    }
}
