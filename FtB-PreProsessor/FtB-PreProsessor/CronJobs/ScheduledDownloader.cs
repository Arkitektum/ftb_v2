using FtB_PreProsessor.CronJobs.Base;
using FtB_PreProsessor.InboundQueue;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FtB_PreProsessor
{
    public class ScheduledFormProcessor : CronJobService
    {
        private readonly IOptionsMonitor<ScheduledDownloaderConfig> _options;
        private readonly ILogger<ScheduledFormProcessor> _logger;
        private readonly IInboundQueue _inboundQueue;
        private readonly IFormProcessor _formProcessor;
        private int _excecutionCount = 0;


        public ScheduledFormProcessor(IScheduleConfig<ScheduledFormProcessor> config, IOptionsMonitor<ScheduledDownloaderConfig> options, ILogger<ScheduledFormProcessor> logger, IInboundQueue inboundQueue, IFormProcessor formProcessor)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _options = options;
            _logger = logger;
            _inboundQueue = inboundQueue;
            _formProcessor = formProcessor;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            var queuedForms = _inboundQueue.GetQueuedFormsFor(_options.CurrentValue.ServiceCodes);
            
            _formProcessor.ProcessForms(queuedForms);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
