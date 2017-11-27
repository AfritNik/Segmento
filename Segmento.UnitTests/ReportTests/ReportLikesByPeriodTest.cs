using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Segmento.Domain;
using Moq;
using Tweetinvi.Models;
using System.Threading.Tasks;

namespace Segmento.UnitTests.ReportTests
{
    /// <summary>
    /// Summary description for ReportLikesByPeriodTest
    /// </summary>
    [TestClass]
    public class ReportLikesByPeriodTest
    {       
        [TestMethod]
        public void Can_Update_New_TwitterReportLikesPePeriod()
        {
            //Arrange - create a report
            TwitterReportLikesByPeriod report = new TwitterReportLikesByPeriod();
            report.UserName = "TestName";

            //Arrange - creating a collection of tweets for update
            List<ITweet> tweetsCollection = new List<ITweet>();

            //Arrange - fill the collection from previous step
            for (int hour=1;hour<=3;hour++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    var tweet = new Mock<ITweet>();
                    tweet.Setup(t => t.CreatedAt).Returns(new System.DateTime(2017, 11, 23, hour, 0, 0));
                    tweet.Setup(t => t.FavoriteCount).Returns(i*hour);
                    
                    tweetsCollection.Add(tweet.Object);
                }
            }

            //Act
            report.Update(11, tweetsCollection);

            //Assert - main report
            Assert.AreEqual(report.TweetsCount, 30);
            Assert.AreEqual(report.reportParts.Count, 3);
            Assert.AreEqual(report.ReportAuthorID, 11);
            Assert.AreEqual(report.LikesMedian, 9);
            Assert.AreEqual(report.BestTimePeriod, 3);

            //Assert - first part
            Assert.AreEqual(report.reportParts[0].UserName,"TestName");
            Assert.AreEqual(report.reportParts[0].TweetsCount, 10);
            Assert.AreEqual(report.reportParts[0].ReportAuthorID, 11);
            Assert.AreEqual(report.reportParts[0].MainReport, report);
            Assert.AreEqual(report.reportParts[0].LikesMedian, 5.5);
            Assert.AreEqual(report.reportParts[0].LikesCount, 55);
            Assert.AreEqual(report.reportParts[0].Hour, 1);

            //Assert - second part
            Assert.AreEqual(report.reportParts[1].UserName, "TestName");
            Assert.AreEqual(report.reportParts[1].TweetsCount, 10);
            Assert.AreEqual(report.reportParts[1].ReportAuthorID, 11);
            Assert.AreEqual(report.reportParts[1].MainReport, report);
            Assert.AreEqual(report.reportParts[1].LikesMedian, 11);
            Assert.AreEqual(report.reportParts[1].LikesCount, 110);
            Assert.AreEqual(report.reportParts[1].Hour, 2);

            //Assert - third part
            Assert.AreEqual(report.reportParts[2].UserName, "TestName");
            Assert.AreEqual(report.reportParts[2].TweetsCount, 10);
            Assert.AreEqual(report.reportParts[2].ReportAuthorID, 11);
            Assert.AreEqual(report.reportParts[2].MainReport, report);
            Assert.AreEqual(report.reportParts[2].LikesMedian, 16.5);
            Assert.AreEqual(report.reportParts[2].LikesCount, 165);
            Assert.AreEqual(report.reportParts[2].Hour, 3);
        }

        [TestMethod]
        public void Update_New_TwitterReportLikesPePeriod_With_Invalid_UserName()
        {
            //Arrange - create a report
            TwitterReportLikesByPeriod report = new TwitterReportLikesByPeriod();

            //Arrange - set incorrect name
            report.UserName = "!It's Incorrect name";

            //Act
            bool result = TweetsReportGenerator.UpdateReport(report);

            //Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public async Task UpdateAsync_New_TwitterReportLikesPePeriod_With_Invalid_UserName()
        {
            //Arrange - create a report
            TwitterReportLikesByPeriod report = new TwitterReportLikesByPeriod();

            //Arrange - set incorrect name
            report.UserName = "!It's Incorrect name";

            //Act
            bool result = await TweetsReportGenerator.UpdateReportAsync(report);

            //Assert
            Assert.AreEqual(result, false);
        }
       
    }
}
