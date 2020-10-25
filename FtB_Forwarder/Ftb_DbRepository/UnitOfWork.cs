using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ftb_Repositories
{
    public class DbUnitOfWork
    {
        private readonly ILogger<DbUnitOfWork> _logger;

        public DbUnitOfWork(ILogger<DbUnitOfWork> logger, ILogEntryRepository logEntryRepository, IFormMetadataRepository formMetadataRepository, IDistributionFormRepository distributionFormRepository)
        {
            _logger = logger;
            LogEntries = logEntryRepository;
            FormMetadata = formMetadataRepository;
            DistributionForms = distributionFormRepository;
        }

        private string archiveReference;

        public void SetArhiveReference(string archiveReference)
        {
            this.archiveReference = archiveReference;
            FormMetadata.SetArchiveReference(archiveReference);
            DistributionForms.SetArchiveReference(archiveReference);
            LogEntries.SetArchiveReference(archiveReference);
        }
        public IDistributionFormRepository DistributionForms { get; }
        public IFormMetadataRepository FormMetadata { get; }
        public ILogEntryRepository LogEntries { get; }

        public void Save()
        {
            FormMetadata.Save();
            DistributionForms.Save();
            LogEntries.Save();
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
