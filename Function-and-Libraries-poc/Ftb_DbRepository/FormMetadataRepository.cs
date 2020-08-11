using Ftb_DbModels;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftb_DbRepository
{
    public class FormMetadataRepository : IFormMetadataRepository
    {
        private readonly FtbDbContext ftbDbContext;

        public FormMetadataRepository(FtbDbContext ftbDbContext)
        {
            this.ftbDbContext = ftbDbContext;
        }

        public FormMetadata GetByReference(string archiveReference)
        {
            var result = ftbDbContext.FormMetadata.Where(f => f.ArchiveReference == archiveReference).FirstOrDefault();

            return result;
        }
    }
}
