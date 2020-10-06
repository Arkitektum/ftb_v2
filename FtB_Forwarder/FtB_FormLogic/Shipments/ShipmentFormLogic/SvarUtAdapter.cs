using Microsoft.Extensions.Logging;

namespace FtB_FormLogic
{
    public class SvarUtAdapter : ISvarUtAdapter
    {
        private readonly ILogger<SvarUtAdapter> _logger;

        public SvarUtAdapter(ILogger<SvarUtAdapter> logger)
        {
            _logger = logger;
        }
        public void Send(SvarUtPayload payload)
        {
            _logger.LogDebug("Sender noko greier til SvarUt");
        }
    }
}
