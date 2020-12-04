﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public abstract class ShipmentSendLogic<T> : SendLogic<T>
    {
        private readonly ISvarUtAdapter svarUtAdapter;

        protected override Receiver Receiver { get => base.Receiver; set => base.Receiver = value; }


        public ShipmentSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, ISvarUtAdapter svarUtAdapter, DbUnitOfWork dbUnitOfWork) : 
            base(repo, tableStorage, log, dbUnitOfWork)
        {
            this.svarUtAdapter = svarUtAdapter;
        }

        public override  async Task<ReportQueueItem> ExecuteAsync(SendQueueItem sendQueueItem)
        {
            var t = await base.ExecuteAsync(sendQueueItem);

            //Map fra FormData til SvarUtPayload 
            svarUtAdapter.Send(new SvarUtPayload() { ReceiverId = Receiver.Id, ReceiverType = Receiver.Type.ToString(), BodyText = "formadata mapping inn i HTML greier sikkert.." });

            return t;
        }
    }
}
