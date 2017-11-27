using Segmento.Domain.Core.Helpers;
using System;
using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Models;

namespace Segmento.WebUI.Controllers
{
    public class TwitterAuthController : Controller
    {
        IAuthenticatedUser user;
        public PartialViewResult Index(IAuthenticatedUser user)
        {
            return PartialView(user);
        }

        /// <summary>
        /// Action for redirecting user to twiiter auth page.
        /// </summary>
        public ActionResult TwitterAuth()
        {
            var appCreds = AuthenticationHelper.GetCreds();
            var redirectURL = "http://" + Request.Url.Authority + "/TwitterAuth/ValidateTwitterAuth";
            var authenticationContext = AuthFlow.InitAuthentication(appCreds, redirectURL);
            return new RedirectResult(authenticationContext.AuthorizationURL);
        }

        /// <summary>
        /// Action after user authentication. If success, redirect to Asministration page (/Index).
        /// Otherwise, redirects to Home page (/List).
        /// </summary>
        public ActionResult ValidateTwitterAuth()
        {
            user = null;
            try
            {
                string verifierCode = Request.Params.Get("oauth_verifier");
                string authorizationId = Request.Params.Get("authorization_id");

                if (!string.IsNullOrEmpty(verifierCode))
                {
                    var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId);
                    user = Tweetinvi.User.GetAuthenticatedUser(userCreds);                    
                }
            }
            catch (NullReferenceException ex)
            {
                //Unsuccessful auth. Show message to user or write to log.
            }
            catch (Exception ex)
            {
                //Unknown error. Show ex.Message to user or write to log.
            }
            finally
            {
                AuthenticationHelper.SetCurrentUser(user);
                Session["User"] = user;
            }            

            string redirectURL;
            if (user == null)
                redirectURL = "http://" + Request.Url.Authority + "/ReportLikesByPeriod/List";
            else
                redirectURL = "http://" + Request.Url.Authority + "/ReportLikesByPeriod/Index";

            return new RedirectResult(redirectURL);
        }
        
    }
}