using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ProcessStrategies
{
    public class DefaultDistributionSendStrategy : SendStrategyBase
    {
        private readonly IPrefillService _prefillService;

        public DefaultDistributionSendStrategy(IFormDataRepo repo, ITableStorage tableStorage, IPrefillService prefillService, ILogger<DefaultDistributionSendStrategy> log) : base(repo, tableStorage, log) {
            _prefillService = prefillService;
        }

        public override ReportQueueItem Exceute(SendQueueItem sendQueueItem)
        {
            Console.WriteLine($"DefaultDistributionSendStrategy: { FormLogicBeingProcessed.ArchiveReference }");

            // Get prefill data generated from formlogic
            // Map to a specific type i.e. Prefill-type for altinn
            FormLogicBeingProcessed.ProcessSendStep(sendQueueItem.Receiver.Id); //Lage og persistere prefill xml

            var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", sendQueueItem.Receiver.Id) };
            repo.AddBytesAsBlob(FormLogicBeingProcessed.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(FormLogicBeingProcessed.DistributionData), metaData);

            // Send using prefill service
            _prefillService.SendPrefill(FormLogicBeingProcessed.ArchiveReference, sendQueueItem.Receiver.Id);

            return base.Exceute(sendQueueItem);
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Henter skjema og vedlegg for DISTRIBUTION");
        }
    }
}
