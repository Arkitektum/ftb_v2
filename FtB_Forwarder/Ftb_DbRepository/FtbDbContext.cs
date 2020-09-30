using Microsoft.EntityFrameworkCore;
using System;

namespace Ftb_DbRepository
{
    public class FtbDbContext : DbContext
    {
        public virtual DbSet<Ftb_DbModels.DistributionForm> DistributionForms { get; set; }
        public virtual DbSet<Ftb_DbModels.FormMetadata> FormMetadata { get; set; }
        public virtual DbSet<Ftb_DbModels.LogEntry> LogEntries { get; set; }
        public virtual DbSet<Ftb_DbModels.PostDistributionMetaData> PostDistributionMetaDatas { get; set; }

        public FtbDbContext(DbContextOptions<FtbDbContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("some connectionstring");
            }
        }

    }
}
