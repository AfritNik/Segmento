using System.Collections.Generic;
using System.Threading.Tasks;

namespace Segmento.Domain.Abstract
{
    public interface IReportRepository
    {
        IEnumerable<TwitterReportLikesByPeriod> Reports { get; }
        IEnumerable<TwitterReportPartLikesByHour> ReportParts { get; }

        /// <summary>
        /// Save report to database
        /// </summary>
        /// <param name="report"></param>
        void SaveReport(TwitterReportLikesByPeriod report);

        /// <summary>
        /// Delete report by ID
        /// </summary>
        /// <param name="ID"></param>
        void DeleteReportByID(int ID);

        /// <summary>
        /// Update report and save to database
        /// </summary>
        /// <param name="report"></param>
        void UpdateReport(TwitterReportLikesByPeriod report);

        /// <summary>
        /// Save report asynchronously
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Task SaveReportAsync(TwitterReportLikesByPeriod report);

        /// <summary>
        /// Delete report by ID asynchronously
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task DeleteReportByIDAsync(int ID);

        /// <summary>
        /// Update report and save to database asynchronously
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Task UpdateReportAsync(TwitterReportLikesByPeriod report);
    }
}
