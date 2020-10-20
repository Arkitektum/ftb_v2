using Ftb_DbModels;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq;

namespace Ftb_DbRepository
{
    public interface ILogEntryRepository
    {
        void Complete();
        void Add(LogEntry logEntry);
    }

    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly ILogger<LogEntryRepository> _logger;
        private ConcurrentBag<LogEntry> _logEntries;

        public LogEntryRepository(ILogger<LogEntryRepository> logger)
        {
            _logEntries = new ConcurrentBag<LogEntry>();
            _logger = logger;
        }

        public void Add(LogEntry logEntry)
        {
            _logEntries.Add(logEntry);
        }

        public void Complete()
        {
            _logger.LogInformation($"Persists {_logEntries.Count()} logentries using REST API");
        }
    }
}