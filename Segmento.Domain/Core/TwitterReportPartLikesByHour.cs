using Segmento.Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using Segmento.Domain.Core.Helpers;
using Tweetinvi.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Segmento.Domain
{
    public class TwitterReportPartLikesByHour: TwitterReport
    {
        /// <summary>
        /// ID of parent report
        /// </summary>
        public int MainReportID { get; set; }

        /// <summary>
        /// Parent report
        /// </summary>
        public TwitterReportLikesByPeriod MainReport { get; set; }

        /// <summary>
        /// The time period (hour) on with current report was compiled
        /// e.x. Hour = 1, then period 01:00-02:00
        /// </summary>
        public int Hour { get; set; }
        
        /// <summary>
        /// Display Hour property in string format
        /// </summary>
        public string HourText
        {
            get { return Hour.ToString("D2") + ":00 - " + (Hour+1).ToString("D2") + ":00"; }
        }

        /// <summary>
        /// Total amout of favorites for current user
        /// </summary>
        public int LikesCount { get; set; }


        /// <summary>
        /// Twitter favorites median
        /// </summary>
        ///<exception cref="InvalidOperationException">Thrown if collection has tweets created in different hours</exception>
        [DisplayFormat(NullDisplayText = "NaN")]
        public double? LikesMedian { get; set; }
        public override void Update(IEnumerable<ITweet> collection)
        {
            base.Update(collection);

            if (collection.Count() == 0) return;
            
            Hour = collection.ElementAt(0).CreatedAt.Hour;
            if (collection.Any(r => r.CreatedAt.Hour != Hour))
                throw new InvalidOperationException("Collection has tweets created in defferent hours");

            LikesCount = collection.Sum(t => t.FavoriteCount);
            UserName = MainReport.UserName;

            LikesMedian = MathHelper.GetMedian(collection.Select(t => t.FavoriteCount));
        }


        /// <exception cref="InvalidOperationException">Thrown if collection has tweets created in different hours</exception>
        public override void Update(long authorID, IEnumerable<ITweet> collection)
        {
            base.Update(authorID, collection);

            if (collection == null || collection.Count() == 0)
            {
                LikesCount = 0;
                LikesMedian = null;
                return;
            }
            
            Hour = collection.ElementAt(0).CreatedAt.Hour;
            if (collection.Any(r => r.CreatedAt.Hour != Hour))
                throw new InvalidOperationException("Collection has tweets created in different hours.");
            
            LikesCount = collection.Sum(t => t.FavoriteCount);
            UserName = MainReport.UserName;

            LikesMedian = MathHelper.GetMedian(collection.Select(t => t.FavoriteCount));
        }        
    }
}
