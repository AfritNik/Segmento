using Segmento.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Segmento.Domain
{
    public static class TweetsReportGenerator
    {
        /// <summary>
        /// Max amount of requested tweets
        /// </summary>
        private static readonly int m_maxNumberOfTweets = 1000;

        /// <summary>
        /// Request tweets attempts count
        /// </summary>
        private static readonly int m_requestTweetsAttempts = 3;


        /// <summary>
        /// Request coolection of tweets
        /// </summary>
        /// <param name="user">Twitter user</param>
        /// <param name="count">Requested count. It should be more 0, but less 200. 200 by default.</param>
        /// <param name="parameters">Parameters of the request</param>
        /// <returns></returns>
        private static IEnumerable<ITweet> RequestTweets(IUser user, int count = 200, IUserTimelineParameters parameters = null)
        {
            if (user == null || count <= 0) return null;
            IEnumerable<ITweet> result = null;

            int attempts = m_requestTweetsAttempts;

            while (attempts > 0 && result == null)
            {
                if (parameters == null)
                    result = Timeline.GetUserTimeline(user, count);
                else
                    result = Timeline.GetUserTimeline(user, parameters);

                attempts--;
            }

            return result;
        }


        /// <summary>
        /// Get tweets from twitter for user
        /// </summary>
        public static IEnumerable<ITweet> GetTweetsForUser(IUser user)
        {
            if (user == null)
                throw new InvalidOperationException("User can not be null");
            var tweets = new List<ITweet>();

            var receivedTweets = RequestTweets(user, 200);
            if (receivedTweets == null)
            {
                return null;
            }
            int requiredTweetsCount = Math.Min(user.StatusesCount, m_maxNumberOfTweets);
            tweets.AddRange(receivedTweets);
            while (tweets.Count < requiredTweetsCount && receivedTweets.Count() != 0)
            {
                var oldestTweet = tweets.Min(x => x.Id);
                var userTimelineParameter = Timeline.CreateUserTimelineParameter();
                userTimelineParameter.MaxId = oldestTweet;
                int remainder = requiredTweetsCount - tweets.Count;
                userTimelineParameter.MaximumNumberOfTweetsToRetrieve = remainder > 200 ? 200 : remainder;


                receivedTweets = RequestTweets(user, 200, userTimelineParameter);
                if (receivedTweets != null)
                    tweets.AddRange(receivedTweets);
                else break;
            }

            return tweets.Distinct();
        }

        /// <summary>
        /// Request new collection of tweets and update the report
        /// </summary>
        public static bool UpdateReport(TwitterReport report)
        {
            IUser user = null;
            try
            {
                user = User.GetUserFromScreenName(report.UserName);
            }
            catch (TwitterNullCredentialsException)
            {
                //Write ex to log
                return false;
            }
            //var user = User.GetUserFromScreenName(report.UserName);
            if (user == null) return false;

            var receivedTweets = GetTweetsForUser(user);

            if (receivedTweets == null) return false;

            if (Settings.CurrentUser != null)
                report.Update(Settings.CurrentUser.Id, receivedTweets);
            else
                report.Update(receivedTweets);

            return true;
        }

        /// <summary>
        /// Request new collection of tweets and update the report asynchronously
        /// </summary>
        public static async Task<bool> UpdateReportAsync(TwitterReport report)
        {
            IUser user = null;
            try
            {
                user = User.GetUserFromScreenName(report.UserName);
            }
            catch (TwitterNullCredentialsException)
            {
                //Write ex to log
                return false;
            }
            if (user == null) return false;

            var receivedTweets = await Task.Run(() => GetTweetsForUser(user));

            if (receivedTweets == null) return false;

            if (Settings.CurrentUser != null)
                report.Update(Settings.CurrentUser.Id, receivedTweets);
            else
                report.Update(receivedTweets);
            return true;
        }
    }
}
