using FtB_PreProsessor.OutboundQueue;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FtB_PreProsessor
{
    public class FormProcessor : IFormProcessor
    {
        private readonly ILogger<FormProcessor> _logger;
        private readonly IQueueClient _outboundQueueClient;

        public FormProcessor(ILogger<FormProcessor> logger, IQueueClient outboundQueueClient)
        {
            _logger = logger;
            _outboundQueueClient = outboundQueueClient;
        }

        public async Task ProcessForms(IEnumerable<QueuedForm> queuedForms)
        {
            foreach (var item in queuedForms)
            {
                _logger.LogInformation("{queuedFormReference}", item.Reference);

                //validate

                //persist data

                //create work message and enqueue it
                
                //Split into work messages
                _outboundQueueClient.QueueFormForProcessing(new QueueMessage() { Reference = item.Reference });
            }

        }
    }
}
