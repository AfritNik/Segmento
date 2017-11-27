using Segmento.Domain;
using Segmento.Domain.Database;
using System.Linq;
using System.Web.Mvc;

namespace Segmento.UI.Controllers
{
    public class HomeController : Controller
    {
        private ReportContext db = new ReportContext();

        // GET: Home
        //public ActionResult Index()
        //{
        //    return View();
        //}
        

        // GET: TwitterReportLikesByPeriods
        public ActionResult Index()
        {
            return View(db.ReportLikesByPeriod.ToList());
        }
        //[HttpPost]
        //public ActionResult Index(TwitterReportLikesByPeriod request)
        //{
        //    TwitterReportLikesByPeriod report = new TwitterReportLikesByPeriod();
        //    TweetsReportGenerator.UpdateReport(request.UserName, report);

        //    using (var ctx = new ReportContext())
        //    {
        //        ctx.ReportLikesByPeriod.Add(report);
        //        foreach (var reportPart in report.reportParts)
        //        {
        //            ctx.ReportPartLikesByHour.Add(reportPart);
        //        }

        //        ctx.SaveChanges();
        //    }
        //    return View();
        //}
    }
}