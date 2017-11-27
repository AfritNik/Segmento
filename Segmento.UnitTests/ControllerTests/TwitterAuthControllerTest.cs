using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Segmento.WebUI.Controllers;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.Web.Routing;
using Tweetinvi;
using Segmento.Domain;
using Tweetinvi.Models;

namespace Segmento.UnitTests.ControllerTests
{
    /// <summary>
    /// Summary description for TwitterAuthControllerTest
    /// </summary>
    [TestClass]
    public class TwitterAuthControllerTest
    {
        [TestMethod]
        public void TwitterAuth_Test_Redirect_Result()
        {
            //Arrange - setting twitter credentials
            Settings.Initialize("Z450BwWnE34tI0GuVEiTwK5Hu", "Vyxpza3PdQzfJoxIkBiDZn3wupigork4RYC3it3crwWpKxE8tY",
                "927601496631504896-LfYfVpI43JxLgitHgLK2YYoXbVhyGoe", "Z7wh2ygDSybQ8bePjswfGRlKO99psvqGTj2MXT5AWb3ab");

            //Arrange - create a controller and context
            TwitterAuthController controller = new TwitterAuthController();
            var context = new Mock<HttpContextBase>();

            //Arrange - create a session
            var session = new Mock<HttpSessionStateBase>();
            Uri uri = new Uri("http://localhost:50067");
            context.Setup(x => x.Request.Url).Returns(uri);
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());

            //Arrange - set a controller context
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            
            //Act
            ActionResult result = controller.TwitterAuth();
            //Act
            string resultUrl = "";
            if (result is RedirectResult)
                resultUrl = ((RedirectResult)result).Url;

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            StringAssert.StartsWith(resultUrl, "https://api.twitter.com/oauth/authorize?");
        }

        [TestMethod]
        public void ValidateTwitterAuth_Test_Redirect_Result_EmptyUser()
        {   
            //Arrange - create a controller and context
            TwitterAuthController controller = new TwitterAuthController();
            var context = new Mock<HttpContextBase>();

            //Arrange - create a session
            var session = new Mock<HttpSessionStateBase>();
            Uri uri = new Uri("http://localhost:50067");
            context.Setup(x => x.Request.Url).Returns(uri);
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());

            //Arrange - set a controller context
            controller.ControllerContext = new ControllerContext(requestContext, controller);

            //Arrange - create mock of user and set to Setteings.CurrentUser
            IAuthenticatedUser user = Mock.Of<IAuthenticatedUser>();
            Settings.CurrentUser = user;

            //Act
            ActionResult result = controller.ValidateTwitterAuth();
            //Act
            string resultUrl = "";
            if (result is RedirectResult)
                resultUrl = ((RedirectResult)result).Url;

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.IsNull(Settings.CurrentUser);
            StringAssert.StartsWith(resultUrl, "http://localhost:50067/ReportLikesByPeriod/List");
        }
    }
}
