using System.Data.Entity;

namespace Segmento.Domain.Database
{
    public class TwitterReportContext : DbContext
    {
        public TwitterReportContext() : base("TwitterReportContext") { }
              
        public DbSet<TwitterReportLikesByPeriod> ReportLikesByPeriod { get; set; }
        public DbSet<TwitterReportPartLikesByHour> ReportPartLikesByHour { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TwitterReportPartLikesByHour>().HasRequired(c => c.MainReport)
            .WithMany(p => p.reportParts);

            base.OnModelCreating(modelBuilder);
        }
    }
}
