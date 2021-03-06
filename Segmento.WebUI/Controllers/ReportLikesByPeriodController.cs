﻿using Segmento.Domain;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Segmento.WebUI.Infrastructure.Attributes;
using Segmento.Domain.Abstract;
using System.Threading.Tasks;
using Tweetinvi.Models;
using System.Web;
using System;
using System.Collections.Generic;
using Segmento.Domain.Database;

namespace Segmento.WebUI.Controllers
{
    public class ReportLikesByPeriodController : AsyncController
    {
        private IReportRepository repository = EFReportRepository.GetInstance();
        //private IReportRepository repository = new EFReportRepository();

        //public ReportLikesByPeriodController(IReportRepository reportRepository)
        //{
        //    this.repository = reportRepository;
        //}
        public ReportLikesByPeriodController()
        {
            
        }
        /// <summary>
        /// Return Administration page for authorized user
        /// </summary>
        [AuthorizeTwitterUser]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Update new report, save to database and return Administration page for authorized user
        /// </summary>
        [HttpPost]
        [AuthorizeTwitterUser]
        public async Task<ActionResult> Index([Bind(Include = "UserName")] TwitterReportLikesByPeriod twitterReportLikesByPeriod)
        {
            if (await TweetsReportGenerator.UpdateReportAsync(twitterReportLikesByPeriod))
                await repository.SaveReportAsync(twitterReportLikesByPeriod);

            ModelState.Clear();
            return View();
        }

        /// <summary>
        /// Return home page
        /// </summary>
        public ActionResult List()
        {
            IEnumerable<TwitterReportLikesByPeriod> report = repository.Reports.OrderBy(r => r.LastUpdateDate);
            return View(report.Skip(Math.Max(0, report.Count() - 10)));
        }

        /// <summary>
        /// Returns partial view with report's table for authorized user
        /// </summary>
        [AuthorizeTwitterUser]
        public ActionResult UpdateReports()
        {
            return PartialView(GetLastUserReports(10));
        }

        /// <summary>
        /// Find report by id and show table with details
        /// </summary>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(repository.ReportParts.Where(rp => rp.MainReportID == id).OrderBy(rp => rp.Hour));
        }

        /// <summary>
        /// Find report by id, update and save to database asynchronously. 
        /// Avalilable only for authorized user.
        /// </summary>
        [AuthorizeTwitterUser]
        public async Task<ActionResult> UpdateAsync(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TwitterReportLikesByPeriod twitterReportLikesByPeriod = repository.Reports.FirstOrDefault(r => r.ID == id);

            if (twitterReportLikesByPeriod != null)
                await repository.UpdateReportAsync(twitterReportLikesByPeriod);

            return PartialView("UpdateReports", GetLastUserReports(10));
        }

        /// <summary>
        /// Find report by id and delete from database asynchronously. 
        /// Avalilable only for authorized user.
        /// </summary>
        [AuthorizeTwitterUser]        
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            await repository.DeleteReportByIDAsync(id.Value);

            return PartialView("UpdateReports", GetLastUserReports(10));
        }

        /// <summary>
        /// Return current user from session
        /// </summary>
        public static IAuthenticatedUser GetCurrentUser(HttpSessionStateBase session)
        {
            IAuthenticatedUser user = session["User"] as IAuthenticatedUser;
            if (user == null)
                return null;
            return user;
        }

        /// <summary>
        /// Return last N reports generated by current user from repository
        /// </summary>
        private IEnumerable<TwitterReportLikesByPeriod> GetLastUserReports(int ReportsCount)
        {
            IEnumerable<TwitterReportLikesByPeriod> userReports = repository.Reports.Where(r => r.ReportAuthorID == GetCurrentUser(Session).Id).OrderBy(r => r.LastUpdateDate);
            return userReports.Skip(Math.Max(0, userReports.Count() - ReportsCount));
        }

    }
}