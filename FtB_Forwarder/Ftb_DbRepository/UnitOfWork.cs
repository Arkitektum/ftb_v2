using System;
using System.Collections.Generic;
using System.Text;

namespace Ftb_DbRepository
{
    public class DbUnitOfWork// : IDisposable
    {
        private readonly ILogEntryRepository _logEntryRepository;
        

        public DbUnitOfWork(ILogEntryRepository logEntryRepository)
        {
            _logEntryRepository = logEntryRepository;
        }

        //private readonly IFormMetadataRepository _formMetadataRepository;
        //public IFormMetadataRepository FormMetadata
        //{
        //    get { return _formMetadataRepository; }
        //}

        public ILogEntryRepository LogEntries
        {
            get { return _logEntryRepository; }
        }

        public void Save()
        {
            //_formMetadataRepository.Complete();
            _logEntryRepository.Complete();
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
