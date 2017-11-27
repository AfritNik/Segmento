using Segmento.Domain.Abstract;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Segmento.Domain.Database
{
    public class EFReportRepository : IReportRepository
    {
        private TwitterReportContext context = new TwitterReportContext();

        private EFReportRepository()
        {
            var parts = context.ReportLikesByPeriod.Include(r => r.reportParts);
            parts.ToListAsync();
        }

        private static EFReportRepository m_Instance;

        public static EFReportRepository GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new EFReportRepository();
            return m_Instance;
        }

        public IEnumerable<TwitterReportLikesByPeriod> Reports
        { get { return context.ReportLikesByPeriod; } }
        public IEnumerable<TwitterReportPartLikesByHour> ReportParts
        { get { return context.ReportPartLikesByHour; } }

        
        public void SaveReport(TwitterReportLikesByPeriod report)
        {
            if (report.ID == 0)
            {
                context.ReportLikesByPeriod.Add(report);
                if (report.reportParts != null && report.reportParts.Count > 0)
                    context.ReportPartLikesByHour.AddRange(report.reportParts);
                context.SaveChanges();
            }
            else
            {
                UpdateReport(report);
                return;
            }
        }
        public void UpdateReport(TwitterReportLikesByPeriod report)
        {
            if (report == null) return;
            TwitterReportLikesByPeriod dbEntry = context.ReportLikesByPeriod.Find(report.ID);
            if (dbEntry == null) return;
            IEnumerable<TwitterReportPartLikesByHour> oldCollection = dbEntry.reportParts;
            TweetsReportGenerator.UpdateReport(dbEntry);
            if (oldCollection != null)
                context.ReportPartLikesByHour.RemoveRange(oldCollection);
            context.ReportPartLikesByHour.AddRange(dbEntry.reportParts);
            context.SaveChanges();
        }
        public void DeleteReportByID(int ID)
        {
            TwitterReportLikesByPeriod dbEntry = context.ReportLikesByPeriod.Find(ID);
            if (dbEntry == null) return;
            context.ReportPartLikesByHour.RemoveRange(dbEntry.reportParts);
            context.ReportLikesByPeriod.Remove(dbEntry);
            context.SaveChanges();
        }

        public async Task SaveReportAsync(TwitterReportLikesByPeriod report)
        {
            if (report.ID == 0)
            {
                context.ReportLikesByPeriod.Add(report);
                if (report.reportParts != null && report.reportParts.Count > 0)
                    context.ReportPartLikesByHour.AddRange(report.reportParts);
                await context.SaveChangesAsync();
            }
            else
            {
                await UpdateReportAsync(report);
            }
        }
        public async Task UpdateReportAsync(TwitterReportLikesByPeriod report)
        {
            if (report == null) return;
            TwitterReportLikesByPeriod dbEntry = context.ReportLikesByPeriod.Find(report.ID);
            if (dbEntry == null) return;
            IEnumerable<TwitterReportPartLikesByHour> oldCollection = dbEntry.reportParts;
            await TweetsReportGenerator.UpdateReportAsync(dbEntry);
            if (oldCollection != null)
                context.ReportPartLikesByHour.RemoveRange(oldCollection);
            context.ReportPartLikesByHour.AddRange(dbEntry.reportParts);
            //db.ReportLikesByPeriod.Add(twitterReportLikesByPeriod);
            await context.SaveChangesAsync();
        }
        public async Task DeleteReportByIDAsync(int ID)
        {
            TwitterReportLikesByPeriod dbEntry = context.ReportLikesByPeriod.Find(ID);
            if (dbEntry == null) return;
            context.ReportPartLikesByHour.RemoveRange(dbEntry.reportParts);
            context.ReportLikesByPeriod.Remove(dbEntry);
            await context.SaveChangesAsync();
        }
    }
}
