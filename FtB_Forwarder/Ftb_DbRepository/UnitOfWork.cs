using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

        private string _archiveReference;

        public void SetArhiveReference(string archiveReference)
        {
            _logger.LogDebug("Setting archive reference in unit of work");
            _archiveReference = archiveReference;
            FormMetadata.SetArchiveReference(_archiveReference);
            DistributionForms.SetArchiveReference(_archiveReference);
            LogEntries.SetArchiveReference(_archiveReference);
        }
        public IDistributionFormRepository DistributionForms { get; }
        public IFormMetadataRepository FormMetadata { get; }
        public ILogEntryRepository LogEntries { get; }

        public async Task Save()
        {
            await FormMetadata.Save();
            await DistributionForms.Save();
            await LogEntries.Save();
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
