using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public abstract class ShipmentSendLogic<T> : SendLogic<T>
    {
        private readonly ISvarUtAdapter svarUtAdapter;

        protected override Receiver Receiver { get => base.Receiver; set => base.Receiver = value; }


        public ShipmentSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, ISvarUtAdapter svarUtAdapter) : base(repo, tableStorage, log)
        {
            this.svarUtAdapter = svarUtAdapter;
        }

        public override  ReportQueueItem Execute(SendQueueItem input)
        {
            var t = base.Execute(input);

            //Map fra FormData til SvarUtPayload 
            svarUtAdapter.Send(new SvarUtPayload() { ReceiverId = Receiver.Id, ReceiverType = Receiver.Type.ToString(), BodyText = "formadata mapping inn i HTML greier sikkert.." });

            return t;
        }
    }
}
