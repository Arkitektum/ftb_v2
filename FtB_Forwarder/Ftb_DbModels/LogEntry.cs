using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Ftb_DbModels
{
    public class LogEntry
    {
        public const string Info = "Info";
        public const string Error = "Error";
        public const bool InternalMsg = true;
        public const bool ExternalMsg = false;

        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        private string _archiveReference;
        public string ArchiveReference { get { return _archiveReference; } set { _archiveReference = value.ToUpper(); } }
        public string Message { get; set; }
        public string Type { get; set; }
        
        public Boolean OnlyInternal { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// Identifier of the event that occured. See Event class
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Duration of the current event in milliseconds
        /// </summary>
        public long? EventDuration { get;set; }

        public LogEntry()
        {

        }

        // TODO - Legg inn Type på alle LogEntries
        public LogEntry(string archiveReference, string message, string type = "")
        {
            ArchiveReference = archiveReference;
            Message = message;
            Type = type;
            Timestamp = DateTime.Now;
        }

        public LogEntry(string archiveReference, string message, string type, Boolean onlyInternal)
        {
            ArchiveReference = archiveReference;
            Message = message;
            Type = type;
            Timestamp = DateTime.Now;
            OnlyInternal = onlyInternal;
        }

        //public static LogEntry NewInfo(string archiveReference, string message)
        //{
        //    return new LogEntry(archiveReference, message, Info);
        //}

        //public static LogEntry NewInfo(string archiveReference, string message, string eventId)
        //{
        //    return new LogEntry(archiveReference, message, Info)
        //    {
        //        EventId = eventId
        //    };
        //}
        //public static LogEntry NewInfoInternal(string archiveReference, string message, string eventId)
        //{
        //    return new LogEntry(archiveReference, message, Info, InternalMsg)
        //    {
        //        EventId = eventId,
        //    };
        //}

        //public static LogEntry NewErrorInternal(string archiveReference, string message, string eventId)
        //{
        //    return new LogEntry(archiveReference, message, Error, InternalMsg)
        //    {
        //        EventId = eventId
        //    };
        //}

        //public static LogEntry NewError(string archiveReference, string message)
        //{
        //    return new LogEntry(archiveReference, message, Error);
        //}

        //public static LogEntry NewError(string archiveReference, string message, string eventId, Stopwatch stopWatch)
        //{
        //    return new LogEntry(archiveReference, message, Error)
        //    {
        //        EventId = eventId,
        //        EventDuration = stopWatch.ElapsedMilliseconds
        //    };
        //}

        //public static LogEntry NewError(string archiveReference, string message, string eventId)
        //{
        //    return new LogEntry(archiveReference, message, Error)
        //    {
        //        EventId = eventId,
        //    };
        //}

        public string ShortMessage()
        {
            string shortMessage = Message;
            if (Message.Length > 110)
            {
                shortMessage = Message.Substring(0, 110) + "...";
            }
            return shortMessage;
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Message);
            builder.Append(", ArchiveReference: ").Append(ArchiveReference);
            builder.Append(", Type: ").Append(Type);
            if (!string.IsNullOrEmpty(EventId))
                builder.Append(", Event: ").Append(EventId);
            
            if (EventDuration != null)
                builder.Append(", Duration: ").Append(EventDuration).Append("ms");

            return builder.ToString();
        }
    }
}