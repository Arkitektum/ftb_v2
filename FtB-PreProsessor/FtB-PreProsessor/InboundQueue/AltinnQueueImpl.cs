using FtB_PreProsessor.InboundQueue;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor.InboundQueue
{
    /// <summary>
    /// 
    /// </summary>
    public class AltinnQueueImpl : IInboundQueue
    {
        private readonly ILogger<AltinnQueueImpl> _logger;
        private readonly AltinnFormMapper altinnFormMapper;

        public AltinnQueueImpl(ILogger<AltinnQueueImpl> logger, AltinnFormMapper altinnFormMapper)
        {
            _logger = logger;
            this.altinnFormMapper = altinnFormMapper;
        }
        public IEnumerable<QueuedForm> GetQueuedFormsFor(List<string> serviceCodes)
        {
            _logger.LogInformation("Downloading forms for service codes {0}", serviceCodes);

            //Retrieves list from queue
            var queuedAltinnForms = new List<AltinnQueuedForm>();

            for (int i = 0; i < new Random().Next(5,15); i++)
            {
                queuedAltinnForms.Add(new AltinnQueuedForm() { ServiceCode = "1234", ServiceEditionCode = 12, ArchiveReference = $"AR{new Random().Next(1000, 9999).ToString()}" });
            }           

            var queuedForms = altinnFormMapper.MapFrom(queuedAltinnForms);            
            
            return queuedForms;
        }
    }

    public class AltinnQueuedForm
    {
        public string ArchiveReference { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceEditionCode { get; set; }
    }
}
