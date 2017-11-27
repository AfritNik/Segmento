using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;

namespace Segmento.Domain.Abstract
{
    public abstract class TwitterReport
    {
        #region Properties
        private int m_id = 0;
        /// <summary>
        /// Gets or sets unique identifier of object
        /// </summary>
        public virtual int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }
        
        /// <summary>
        /// Gets or sets name of user
        /// </summary>
        public virtual string UserName { get; set; }

        
        /// <summary>
        /// Gets or sets time of last update
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets count of tweets
        /// </summary>
        public int TweetsCount { get; set; }


        /// <summary>
        /// Gets or sets author ID of report
        /// </summary>
        public long ReportAuthorID { get; set; }
        #endregion

        #region Methods
        public virtual void Update(IEnumerable<ITweet> collection)
        {
            LastUpdateDate = DateTime.Now;
            TweetsCount = collection.Count();
        }

        public virtual void Update(long authorID, IEnumerable<ITweet> collection)
        {
            ReportAuthorID = authorID;
            LastUpdateDate = DateTime.Now;
            TweetsCount = collection==null ? 0: collection.Count();
        }
        #endregion
    }
}
