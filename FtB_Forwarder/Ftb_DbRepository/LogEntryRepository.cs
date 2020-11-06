using Ftb_DbModels;
using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq;

namespace Ftb_Repositories
{
    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly ILogger<LogEntryRepository> _logger;
        private readonly LogEntryHttpClient _logEntryClient;
        private ConcurrentBag<LogEntry> _logEntries;
        private static string _archiveReference;
        public LogEntryRepository(ILogger<LogEntryRepository> logger, LogEntryHttpClient logEntryClient)
        {
            _logEntries = new ConcurrentBag<LogEntry>();
            _logger = logger;
            _logEntryClient = logEntryClient;
        }
        public void SetArchiveReference(string archiveReference)
        {
            _archiveReference = archiveReference;
        }

        //private void Add(LogEntry logEntry)
        //{
        //    _logEntries.Add(logEntry);
        //}

        public void Save()
        {
            _logger.LogInformation($"Persists {_logEntries.Count()} logentries using REST API");
            if (_logEntries != null)
                _logEntryClient.Post(_archiveReference, _logEntries);
        }

        public void AddInfo(string message)
        {
            _logEntries.Add(new LogEntry(_archiveReference, message, LogEntry.Info));
        }

        public void AddInfo(string message, string eventId)
        {
            _logEntries.Add(new LogEntry(_archiveReference, message, LogEntry.Info) { EventId = eventId });
        }
        public void AddInfoInternal(string message, string eventId)
        {
            _logEntries.Add(new LogEntry(_archiveReference, message, LogEntry.Info, LogEntry.InternalMsg) { EventId = eventId, });
        }

        public void AddErrorInternal(string message, string eventId)
        {
            _logEntries.Add(new LogEntry(_archiveReference, message, LogEntry.Error, LogEntry.InternalMsg) { EventId = eventId });
        }

        public void AddError(string message)
        {
            _logEntries.Add(new LogEntry(_archiveReference, message, LogEntry.Error));
        }

        public void AddNewError(string message, string eventId)
        {
            _logEntries.Add(new LogEntry(_archiveReference, message, LogEntry.Error) { EventId = eventId, });
        }
    }
}