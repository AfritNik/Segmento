using Segmento.Domain.Abstract;
using Segmento.Domain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tweetinvi.Models;

namespace Segmento.Domain
{
    /// <summary>
    /// Twitter report with favorites statistic for user 
    /// </summary>
    public class TwitterReportLikesByPeriod : TwitterReport
    {
        #region Properties

        /// <summary>
        /// Twitter favorites median
        /// </summary>
        [DisplayFormat(NullDisplayText = "NaN")]
        public double? LikesMedian { get; set; }

        /// <summary>
        /// The time period (hour) with the highest likes (favorites) median
        /// e.x. Hour = 1, then period 01:00-02:00
        /// </summary>
        [DisplayFormat(NullDisplayText = "NaN")]
        public int? BestTimePeriod { get; set; }

        /// <summary>
        /// Display BestTimePeriod property in string format
        /// </summary>
        public string BestTimePeriodText
        {
            get
            {
                if (BestTimePeriod.HasValue)
                    return BestTimePeriod.Value.ToString("D2") + ":00 - " + (BestTimePeriod + 1).Value.ToString("D2") + ":00";
                else return "NaN";
            }
        }

        /// <summary>
        /// Collection of linked reports with information divided by hours
        /// </summary>
        public List<TwitterReportPartLikesByHour> reportParts { get; set; } = new List<TwitterReportPartLikesByHour>();
        #endregion

        #region Methods
        public override void Update(IEnumerable<ITweet> collection)
        {
            base.Update(collection);
            if (collection == null) return;

            var tweetsDevidedByHours = collection.GroupBy(x => x.CreatedAt.Hour);
            reportParts = new List<TwitterReportPartLikesByHour>();
            foreach (var tweetGroup in  tweetsDevidedByHours)
            {
               TwitterReportPartLikesByHour reportPart = new TwitterReportPartLikesByHour();
                reportPart.MainReport = this;
                try
                {
                    reportPart.Update(tweetGroup);
                }
                catch (InvalidOperationException)
                {
                    //Write ex to log
                    continue;
                }
                reportParts.Add(reportPart);
            }
            if (collection.Count()>0)
            {
                LikesMedian = MathHelper.GetMedian(collection.Select(t => t.FavoriteCount));
                BestTimePeriod = reportParts.Count == 0
                    ? 0
                    : reportParts.OrderByDescending(r => r.LikesMedian).FirstOrDefault().Hour;
            }
            else
            {
                LikesMedian = null;
                BestTimePeriod = null;
            }
        }


        public override void Update(long authorID, IEnumerable<ITweet> collection)
        {
            base.Update(authorID, collection);

            if (collection == null) return;

            var tweetsDevidedByHours = collection.GroupBy(x => x.CreatedAt.Hour);
            reportParts = new List<TwitterReportPartLikesByHour>();
            foreach (var tweetGroup in tweetsDevidedByHours)
            {
                TwitterReportPartLikesByHour reportPart = new TwitterReportPartLikesByHour();
                reportPart.MainReport = this;
                try
                {
                    reportPart.Update(authorID, tweetGroup);
                }
                catch (InvalidOperationException)
                {
                    //Write ex to log
                    continue;
                }
                reportParts.Add(reportPart);
            }
            if (collection.Count() > 0)
            {
                LikesMedian = MathHelper.GetMedian(collection.Select(t => t.FavoriteCount));
                BestTimePeriod = reportParts.Count == 0
                    ? 0
                    : reportParts.OrderByDescending(r => r.LikesMedian).FirstOrDefault().Hour;
            }
            else
            {
                LikesMedian = null;
                BestTimePeriod = null;
            }
        }
        #endregion
    }
}
