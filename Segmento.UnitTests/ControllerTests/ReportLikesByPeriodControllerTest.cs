using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Segmento.WebUI.Controllers;
using Moq;
using Segmento.Domain.Abstract;
using System.Web.Mvc;
using Segmento.Domain;
using System.Threading.Tasks;

namespace Segmento.UnitTests.ControllerTests
{
    /// <summary>
    /// Unit tests for ReportLikesByPeriodController from Segmento.WebUI
    /// </summary>
    [TestClass]
    public class ReportLikesByPeriodControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            //Arrange - creating repository mock
            IReportRepository repository = Mock.Of<IReportRepository>();
            
            //Arrange - creating a controller
            ReportLikesByPeriodController controller = new ReportLikesByPeriodController();

            //Action
            ViewResult result = controller.Index() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }
        
    }
}
