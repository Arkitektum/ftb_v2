using Ftb_DbModels;
using System.Linq;

namespace Ftb_Repositories.Interfaces
{
    public interface ILogEntryRepository
    {
        void Save();
        //void Add(LogEntry logEntry);
        void AddInfo(string message);
        void AddInfo(string message, string eventId);
        void AddInfoInternal(string message, string eventId);
        void AddErrorInternal(string message, string eventId);
        void AddError(string message);
        void AddNewError(string message, string eventId);
        void SetArchiveReference(string archiveReference);
    }
}