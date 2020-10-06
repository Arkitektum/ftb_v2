using FtB_Common.Adapters;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic.OTSFormLogic
{
    public abstract class DistributionSendLogic<T> :  SendLogic<T>
    {
        private readonly IPrefillAdapter _prefillAdapter;

        public PrefillData PrefillData { get; set; }
        protected string prefillDataString { get; set; }


        public DistributionSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, IPrefillAdapter prefillAdapter) : base(repo, tableStorage, log)
        {
            _prefillAdapter = prefillAdapter;
        }

        public override ReportQueueItem Execute(SendQueueItem sendQueueItem)
        {
            MapPrefillData(sendQueueItem.Receiver.Id);

            //Execute base logic
            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillCreated));

            //prefillData = 
            PersistPrefill(sendQueueItem);

            SendPrefill(sendQueueItem);

            Distribute(sendQueueItem);

            return new ReportQueueItem() { ArchiveReference = sendQueueItem.ArchiveReference, Receiver = sendQueueItem.Receiver };
        }

        protected virtual void PersistPrefill(SendQueueItem sendQueueItem)
        {
            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
            _repo.AddBytesAsBlob(sendQueueItem.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(PrefillData.XmlDataString), metaData);
            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillPersisted));
        }

        protected virtual void SendPrefill(SendQueueItem sendQueueItem)
        {
            var prefillResult = _prefillAdapter.SendPrefill(PrefillData);// .SendPrefill(FormLogicBeingProcessed.ArchiveReference, sendQueueItem.Receiver.Id);
            switch (prefillResult.ResultType)
            {
                case PrefillResultType.Ok:
                    UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.PrefillSent));
                    break;
                case PrefillResultType.UnkownErrorOccured:
                    break;
                case PrefillResultType.ReservedReportee:
                    break;
                case PrefillResultType.UnableToReachReceiver:
                    break;
                default:
                    break;
            }
        }

        protected virtual void Distribute(SendQueueItem sendQueueItem)
        {
            // Validate if receiver info is sufficient

            // Decrypt

            // Create distributionform 


            // Map  from prefill-data to prefillFormTask
            // Send using prefill service

            // _distributionAdapter.SendDistribution(eitEllerAnnaObjekt);

            // Finally persist distributionform..  and maybe a list of logentries??            

            UpdateReceiverEntity(new ReceiverEntity(sendQueueItem.ArchiveReference, sendQueueItem.Receiver.Id, ReceiverStatusEnum.Sent));
        }



        protected abstract void MapPrefillData(string receiverId);

    }





    //[FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    //public class VarselOppstartPlanarbeidSendLogic : VarselOppstartPlanarbeidLogic, ISendResultProvider
    //{
    //    private readonly ISendStrategy sendStrategy;
    //    private readonly IFormMapper<NabovarselPlanType, SvarPaaNabovarselPlanType> formMapper;

    //    //Problemstilling:
    //    // Skal mappe frå ein type til ein anna og sende den
    //    // Mapping er unik for denne, men utsending er felles for fleire

    //    public VarselOppstartPlanarbeidSendLogic(ISendStrategy sendStrategy, IFormMapper<NabovarselPlanType, SvarPaaNabovarselPlanType> formMapper)
    //    {   
    //        this.sendStrategy = sendStrategy;
    //        this.formMapper = formMapper;
    //    }

    //    private ReportQueueItem reportItem;
    //    public override void ExecuteStrategy(object strategyTrigger)
    //    {
    //        formMapper.Map(base.)
    //        reportItem = sendStrategy.Execute(strategyTrigger as SendQueueItem);
    //    }

    //    public ReportQueueItem GetResult()
    //    {
    //        return reportItem;
    //    }
    //}

    //public class VarselOppstartPlanarbeidReportLogic : VarselOppstartPlanarbeidLogic
    //{
    //    private readonly IReportStrategy reportStrategy;

    //    public VarselOppstartPlanarbeidReportLogic(IReportStrategy reportStrategy)
    //    {
    //        this.reportStrategy = reportStrategy;
    //    }

    //    public override void ExecuteStrategy(object strategyTrigger)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
